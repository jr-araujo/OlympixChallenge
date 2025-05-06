namespace ReentrancyLockVerifier.Core;

public static class OpcodeDefinitions
{
    public static readonly Dictionary<string, (string Name, int PushBytes)> Opcodes = new()
    {
        {"50", ("POP", 0)},
        {"52", ("MSTORE", 0)},
        {"54", ("SLOAD", 0)},
        {"55", ("SSTORE", 0)},
        {"56", ("JUMP", 0)},
        {"57", ("JUMPI", 0)},
        {"5B", ("JUMPDEST", 0)},
        {"60", ("PUSH1", 1)}, {"61", ("PUSH2", 2)}, {"62", ("PUSH3", 3)},
        {"7F", ("PUSH32", 32)},
        {"F1", ("CALL", 0)}, {"F2", ("CALLCODE", 0)},
        {"F4", ("DELEGATECALL", 0)}, {"FA", ("STATICCALL", 0)},
    };
}