using Taki.Game.Cards;
using Taki.Game.Deck;

namespace Taki.Game.GameRules
{
    internal class CardsHandler
    {
        private readonly CardDeck _drawPile;
        private readonly CardDeck _discardPile;

        public CardsHandler(CardDeck drawPile, CardDeck discardPile) 
        {
            _drawPile = drawPile;
            _discardPile = discardPile;
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
            _drawPile.CombineDeckToThis(_discardPile);
            _drawPile.ShuffleDeck();
        }

        public Card? DrawCard()
        {
            if(_drawPile.Count() == 0 && _discardPile.Count() > 1)
            {
                ResetCards();
                DrawFirstCard();
            }

            if(_drawPile.Count() + _discardPile.Count() == 1)
                return null;

            Card top = _drawPile.GetFirst();
            _drawPile.RemoveFirst();

            return top;
        }

        public void DrawFirstCard()
        {
            Card? drawCard = DrawCard();
            if(drawCard != null)
                _discardPile.AddFirst(drawCard);
        }
    }
}
