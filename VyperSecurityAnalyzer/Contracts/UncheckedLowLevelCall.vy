# UncheckedLowLevelCall.vy
@external
def execute_call_operation(target: address, data: Bytes[100]):
    # ❌ raw_call sem checar sucesso
    raw_call(target, data)
