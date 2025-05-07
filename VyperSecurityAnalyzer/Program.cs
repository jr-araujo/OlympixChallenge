using VyperSecurityAnalyzer.Analyzer;
using VyperSecurityAnalyzer.Models;

Console.WriteLine("🏁  Starting Vyper contract analyzer...");

Console.WriteLine(Environment.NewLine);

if (args.Length == 0 || args[0] != "--files")
{
    Console.WriteLine("(*) Wrong parameters...");
    Console.WriteLine("Usage: dotnet run --files \"file1.vy\" \"C:\\file 2.vy\"");
    return;
}

if (args.Length > 0 && args[0] == "--files" && !args.Skip(1).Any())
{
    Console.WriteLine("(**) Please inform at least one Vyper (.vy) file.");
    Console.WriteLine("Usage: dotnet run --files \"file1.vy\" \"C:\\file 2.vy\"");
    return;
}

var filePaths = ParseFilesArgument(args.Skip(1).ToArray());

Dictionary<string, string> ParseFilesArgument(string[] args)
{
    var files = new Dictionary<string, string>();
    foreach (var filePath in args)
    {
        files.Add(Path.GetFileName(filePath), filePath);
    }

    return files;
}

var analyzer = new VulnerabilityAnalyzer();
var issueReports = analyzer.AnalyzeFiles(filePaths);

foreach (var issue in issueReports)
{
    Console.WriteLine($"File: {issue.FileName}");

    if (!issue.Vulnerabilities.Any())
    {
        Console.WriteLine("✅  Safe contract");
        continue;
    }

    foreach (var vulnerability in issue.Vulnerabilities)
    {
        var severityIcon = vulnerability.Severity switch
        {
            Severity.Low => "🤞",
            Severity.High => "❗️",
            Severity.Critical => "☠️",
            Severity.Medium => "💣",
            _ => throw new ArgumentOutOfRangeException($"Unknown severity")
        };
        
        Console.WriteLine($"[{severityIcon}] {vulnerability.FunctionName}: {vulnerability.Vulnerability.ToString()} - {vulnerability.Description}");
    }
}