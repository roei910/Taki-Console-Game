using Microsoft.Extensions.DependencyInjection;
using Taki.Game.Cards;
using Taki.Game.Factories;

namespace Taki.Game.Deck
{
    internal class CardDecksHolder : ICardDecksHolder
    {
        private readonly CardDeck _drawPile;
        private readonly CardDeck _discardPile;

        public CardDecksHolder(IServiceProvider serviceProvider)
        {
            CardDeckFactory factory = serviceProvider.GetRequiredService<CardDeckFactory>();
            _drawPile = factory.GenerateCardDeck(serviceProvider);
            _discardPile = new(serviceProvider);
        }

        public Card GetTopDiscard()
        {
            return _discardPile.GetFirst();
        }

        public void AddDiscardCard(Card card)
        {
            _discardPile.AddFirst(card);
        }

        public void ResetCards()
        {
            _drawPile.CombineFromDeck(_discardPile);
            _drawPile.ShuffleDeck();
        }

        public void ResetCards(List<Card> playerCards)
        {
            _discardPile.AddMany(playerCards);
            ResetCards();
        }

        public Card? DrawCard()
        {
            if (_drawPile.Count() + _discardPile.Count() == 1)
                return null;

            if (_drawPile.Count() == 0 && _discardPile.Count() > 1)
            {
                ResetCards();
                DrawFirstCard();
            }

            Card top = _drawPile.PopFirst();

            return top;
        }

        public void DrawFirstCard()
        {
            Card? drawCard = DrawCard();
            _discardPile.AddFirst(drawCard!);
        }

        public int CountAllCards()
        {
            return _drawPile.Count() + _discardPile.Count();
        }
    }
}
