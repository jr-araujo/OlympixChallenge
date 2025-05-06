# SharedStateBank.vy
# Falha: Uso de estado compartilhado entre usuários

sharedBalance: public(uint256)

@external
@payable
def deposit():
    self.sharedBalance += msg.value

@external
def withdraw(amount: uint256):
    if amount > 0 and self.sharedBalance >= amount:
        send(msg.sender, amount)
        self.sharedBalance -= amount  # ⚠️ Corrida de condição possível
