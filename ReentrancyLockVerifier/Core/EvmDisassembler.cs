namespace ReentrancyLockVerifier.Core;

public class EvmDisassembler
{
    public static List<EvmInstruction> Disassemble(string bytecode)
    {
        var instructions = new List<EvmInstruction>();
        int i = 0;
        while (i < bytecode.Length)
        {
            string hex = bytecode.Substring(i, 2).ToUpper();
            if (OpcodeDefinitions.Opcodes.TryGetValue(hex, out var def))
            {
                instructions.Add(new EvmInstruction(def.Name, i / 2));
                i += 2 + def.PushBytes * 2;
            }
            else
            {
                instructions.Add(new EvmInstruction("UNKNOWN", i / 2));
                i += 2;
            }
        }
        return instructions;
    }
}

public record EvmInstruction(string Opcode, int Position);