# SafeBank.vy
# Banco seguro com proteção contra reentrância via mutex

balances: public(HashMap[address, uint256])
locked: public(bool)

@external
@payable
def deposit():
    self.balances[msg.sender] += msg.value

@external
def withdraw():
    assert not self.locked, "Reentrancy detected"
    self.locked = True  # 🛑 trava antes de executar código crítico

    amount: uint256 = self.balances[msg.sender]
    if amount > 0:
        self.balances[msg.sender] = 0
        send(msg.sender, amount)

    self.locked = False  # 🔓 libera ao final
