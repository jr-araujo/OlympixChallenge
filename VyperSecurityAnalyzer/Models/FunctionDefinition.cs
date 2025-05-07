namespace VyperSecurityAnalyzer.Models;

public class FunctionDefinition
{
    public string Name { get; set; } = string.Empty;
    public List<string> BodyLines { get; set; } = [];
}
