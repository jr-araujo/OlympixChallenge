# SharedStateAuction.vy
highest_bid: public(uint256)
highest_bidder: public(address)

@external
@payable
def bid():
    # ❌ Estado compartilhado alterado de forma insegura
    if msg.value > self.highest_bid:
        # Envia de volta o valor anterior ao antigo vencedor
        if self.highest_bidder != empty(address):
            send(self.highest_bidder, self.highest_bid)

        # Atualiza o estado global
        self.highest_bid = msg.value
        self.highest_bidder = msg.sender