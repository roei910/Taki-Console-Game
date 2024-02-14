using Microsoft.Extensions.DependencyInjection;
using Taki.Game.Cards;
using Taki.Game.Deck;
using Taki.Game.Factories;
using Taki.Game.Handlers;

namespace Taki.Game.GameRules
{
    //TODO: naming => maybe card decks holder???
    internal class CardsHandler : ICardsHandler
    {
        private readonly CardDeck _drawPile;
        private readonly CardDeck _discardPile;

        public CardsHandler(IServiceProvider serviceProvider)
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
            //TODO: naming
            _drawPile.CombineDeckToThis(_discardPile);
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

            //TODO: PopFirst
            Card top = _drawPile.GetFirst();
            _drawPile.RemoveFirst();

            return top;
        }

        public void DrawFirstCard()
        {
            Card? drawCard = DrawCard();
            
            //TODO: is needed?
            if(drawCard != null)
                _discardPile.AddFirst(drawCard);
        }

        public int CountAllCards()
        {
            return _drawPile.Count() + _discardPile.Count();
        }
    }
}
