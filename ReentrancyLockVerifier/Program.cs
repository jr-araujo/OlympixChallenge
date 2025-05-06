using ReentrancyLockVerifier.Core;

if (args.Length == 0)
{
    Console.WriteLine("Usage: dotnet run -- --file <path_to_bytecode_file>");
    return;
}

var fileArgIndex = Array.IndexOf(args, "--file") + 1;
if (fileArgIndex == 0 || fileArgIndex >= args.Length)
{
    Console.WriteLine("Missing --file argument.");
    return;
}

var filePath = args[fileArgIndex];
var bytecode = File.ReadAllBytes(filePath);
var issues = new ContractAnalyzer().Analyze(bytecode);

Console.WriteLine($"\n📄 Analyzing {Path.GetFileName(filePath)}...");
if (issues.Count == 0)
{
    Console.WriteLine("✅ No vulnerabilities detected.");
}
else
{
    Console.WriteLine("❌ Issues found:");
    foreach (var issue in issues)
    {
        Console.WriteLine(" - " + issue.Description);
    }
}