# TxOriginAuth.vy
owner: public(address)

@external
def __init__():
    self.owner = msg.sender

@external
def withdraw():
    # ❌ Verificação insegura com tx.origin
    if tx.origin != self.owner:
        raise "Not authorized"

    send(self.owner, self.balance)
