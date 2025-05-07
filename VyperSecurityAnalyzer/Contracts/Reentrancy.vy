# ReentrancyExample.vy
balances: public(HashMap[address, uint256])

@external
@payable
def deposit():
    self.balances[msg.sender] += msg.value

@external
def withdraw():
    amount: uint256 = self.balances[msg.sender]
    if amount > 0:
        # ❌ Chamada externa antes de zerar o saldo
        send(msg.sender, amount)
        self.balances[msg.sender] = 0  # Estado atualizado depois