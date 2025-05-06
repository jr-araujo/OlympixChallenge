namespace ReentrancyLockVerifier.Core;

public class ReentrancyAnalyzer
{
    private static readonly HashSet<string> ExternalCalls = new() { "CALL", "DELEGATECALL", "CALLCODE", "STATICCALL" };

    public static List<ReentrancyIssue> Analyze(List<EvmInstruction> instructions)
    {
        var issues = new List<ReentrancyIssue>();

        for (int i = 0; i < instructions.Count; i++)
        {
            if (ExternalCalls.Contains(instructions[i].Opcode))
            {
                var recentOps = instructions.Skip(Math.Max(0, i - 10)).Take(10).Select(x => x.Opcode).ToList();
                bool hasSstore = recentOps.Contains("SSTORE");
                if (!hasSstore)
                {
                    issues.Add(new ReentrancyIssue(instructions[i].Opcode, instructions[i].Position));
                }
            }
        }
        return issues;
    }
}

public record ReentrancyIssue(string ExternalCall, int Position)
{
    public string Message => $"Reentrancy risk: External call '{ExternalCall}' at bytecode position {Position} is not preceded by a state update (SSTORE).";
}