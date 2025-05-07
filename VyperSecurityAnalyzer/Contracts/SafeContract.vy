# SafeBank.vy

balances: public(HashMap[address, uint256])
owner: public(address)

@external
def __init__():
    self.owner = msg.sender

@external
@payable
def deposit():
    self.balances[msg.sender] += msg.value

@external
def withdraw(amount: uint256):
    assert self.balances[msg.sender] >= amount, "Saldo insuficiente"

    # Atualiza o saldo ANTES da chamada externa (protege contra reentrância)
    self.balances[msg.sender] -= amount

    # Chamada externa segura
    send(msg.sender, amount)

@external
def check_balance(user: address) -> uint256:
    return self.balances[user]

@external
def only_owner_action():
    assert msg.sender == self.owner, "Apenas o dono pode executar isso"
    # Alguma ação administrativa segura
