namespace ReentrancyLockVerifier.Core;
public class EvmInstruction
{
    public int Offset { get; set; }
    public string Opcode { get; set; } = string.Empty;
}

public static class EvmDisassembler
{
    public static List<EvmInstruction> Disassemble(byte[] bytecode)
    {
        var instructions = new List<EvmInstruction>();
        int i = 0;
        while (i < bytecode.Length)
        {
            byte op = bytecode[i];
            string opcode = GetOpcode(op);
            instructions.Add(new EvmInstruction { Offset = i, Opcode = opcode });
            i++;

            if (op < 0x60 || op > 0x7f) continue; // PUSH1 - PUSH32
            var pushBytes = op - 0x5f;
            i += pushBytes;
        }
        return instructions;
    }

    private static string GetOpcode(byte op)
    {
        return OpcodeTable.TryGetValue(op, out var value) ? value : $"UNKNOWN_{op:X2}";
    }

    private static readonly Dictionary<byte, string> OpcodeTable = new()
    {
        { 0x00, "STOP" },
        { 0x01, "ADD" },
        { 0x02, "MUL" },
        { 0x03, "SUB" },
        { 0x04, "DIV" },
        { 0x10, "LT" },
        { 0x11, "GT" },
        { 0x15, "ISZERO" },
        { 0x14, "EQ" },
        { 0x20, "SHA3" },
        { 0x30, "ADDRESS" },
        { 0x31, "BALANCE" },
        { 0x35, "CALLDATALOAD" },
        { 0x36, "CALLDATASIZE" },
        { 0x37, "CALLDATACOPY" },
        { 0x39, "CODECOPY" },
        { 0x50, "POP" },
        { 0x51, "MLOAD" },
        { 0x52, "MSTORE" },
        { 0x53, "MSTORE8" },
        { 0x54, "SLOAD" },
        { 0x55, "SSTORE" },
        { 0x56, "JUMP" },
        { 0x57, "JUMPI" },
        { 0x60, "PUSH1" },
        { 0x61, "PUSH2" },
        { 0x7f, "PUSH32" },
        { 0xf1, "CALL" },
        { 0xf2, "CALLCODE" },
        { 0xf4, "DELEGATECALL" },
        { 0xfa, "STATICCALL" }
    };
}