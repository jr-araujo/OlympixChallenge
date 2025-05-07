using VyperSecurityAnalyzer.Models;

namespace VyperSecurityAnalyzer.Parser;

using System.Text.RegularExpressions;

public class VyperParser
{
    public List<FunctionDefinition> Parse(string[] lines)
    {
        var functions = new List<FunctionDefinition>();
        FunctionDefinition? currentFunction = null;

        foreach (var line in lines.Select(l => l.Trim()))
        {
            if (line.StartsWith("def "))
            {
                if (currentFunction != null)
                    functions.Add(currentFunction);

                var match = Regex.Match(line, @"def (\w+)\(");
                currentFunction = new FunctionDefinition
                {
                    Name = match.Groups[1].Value
                };
            }

            if (currentFunction != null)
                currentFunction.BodyLines.Add(line);
        }

        if (currentFunction != null)
            functions.Add(currentFunction);

        return functions;
    }
}
