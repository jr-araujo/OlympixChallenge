# NoBalanceCheckBank.vy
# Falha: Envia fundos sem verificar saldo

balances: public(HashMap[address, uint256])

@external
@payable
def deposit():
    self.balances[msg.sender] += msg.value

@external
def withdraw(amount: uint256):
    # ⚠️ Não verifica se o usuário tem saldo suficiente
    send(msg.sender, amount)
    self.balances[msg.sender] -= amount