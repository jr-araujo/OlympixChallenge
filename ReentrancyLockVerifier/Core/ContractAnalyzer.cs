using System.Text.RegularExpressions;

namespace ReentrancyLockVerifier.Core;

public enum VulnerabilityType
{
    Reentrancy,
    MissingBalanceCheck,
    SharedStateRaceCondition
}

public class Vulnerability
{
    public VulnerabilityType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Offset { get; set; }
}

public class ContractAnalyzer
{
    private const string GreaterThan = "GT";
    private const string LessThan = "LT";
    private const string IsZero = "ISZERO";
    private const string Equal = "EQ";
    private const string Balance = "BALANCE";
    private const string SLoad = "SLOAD";
    
    public List<Vulnerability> Analyze(byte[] bytecode)
    {
        var instructions = EvmDisassembler.Disassemble(bytecode);
        var vulnerabilities = new List<Vulnerability>();
    
        bool hasExternalCall = false;
        bool balanceUpdated = false;
    
        for (int i = 0; i < instructions.Count; i++)
        {
            var inst = instructions[i];
            
            if (IsExternalCall(inst))
            {
                hasExternalCall = true;
    
                bool hasBalanceCheck = false;
                for (int j = i - 1; j >= 0; j--)
                {
                    if (instructions[j] is { } instr && HasBalanceCheck(instr))
                    {
                        hasBalanceCheck = true;
                        break;
                    }
                }
    
                if (!hasBalanceCheck)
                {
                    vulnerabilities.Add(new Vulnerability
                    {
                        Type = VulnerabilityType.MissingBalanceCheck,
                        Offset = inst.Offset,
                        Description = $"Missing balance check before external call at offset {inst.Offset}"
                    });
                }
            }
            else if (inst.Opcode == "SSTORE")
            {
                balanceUpdated = true;
            }
        }
    
        if (hasExternalCall && !balanceUpdated)
        {
            vulnerabilities.Add(new Vulnerability
            {
                Type = VulnerabilityType.Reentrancy,
                Offset = -1,
                Description = "Potential reentrancy vulnerability: external call before state update"
            });
        }
    
        foreach (var inst in instructions)
        {
            if (inst.Opcode == "SSTORE")
            {
                // Heuristic: suspect if storing to shared state without context (e.g., no msg.sender)
                vulnerabilities.Add(new Vulnerability
                {
                    Type = VulnerabilityType.SharedStateRaceCondition,
                    Offset = inst.Offset,
                    Description = $"Potential shared state race condition at offset {inst.Offset}"
                });
            }
        }
    
        return vulnerabilities;
    }

    private bool IsExternalCall(EvmInstruction inst)
    {
        string[] externalCallOperations = ["CALL", "CALLCODE", "DELEGATECALL", "STATICCALL"];
        return externalCallOperations.Contains(inst.Opcode);
    }

    private bool HasBalanceCheck(EvmInstruction instruction)
    {
        string[] allowedOperations = [GreaterThan, LessThan, IsZero, Equal, Balance, SLoad];
        return allowedOperations.Contains(instruction.Opcode);
    }
}