using Taki.Data;
using Taki.Dto;
using Taki.Factories;
using Taki.Interfaces;
using Taki.Models.Cards;

namespace Taki.Models.Deck
{
    internal class CardDecksHolder : ICardDecksHolder
    {
        private readonly CardDeck _drawPile;
        private readonly CardDeck _discardPile;
        private readonly CardDeckDatabase _cardDeckDatabase;

        public CardDecksHolder(CardDeckFactory cardDeckFactory, Random random,
            CardDeckDatabase cardDeckDatabase)
        {
            _drawPile = cardDeckFactory.GenerateCardDeck();
            _discardPile = new CardDeck(random);
            _cardDeckDatabase = cardDeckDatabase;
        }

        public Card GetTopDiscard()
        {
            return _discardPile.GetFirst();
        }

        public void AddDiscardCard(Card card)
        {
            _discardPile.AddFirst(card);
            _cardDeckDatabase.AddDiscardCard(card);
        }

        public void ResetCards()
        {
            _drawPile.CombineFromDeck(_discardPile);
            _drawPile.ShuffleDeck();
            _cardDeckDatabase.UpdateAllCards(_discardPile, _drawPile);
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
                Card topDiscard = _discardPile.PopFirst();
                ResetCards();
                _discardPile.AddFirst(topDiscard);
            }

            Card top = _drawPile.PopFirst();
            _cardDeckDatabase.DrawCard(top);

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

        public Card RemoveCardByDTO(CardDto card)
        {
            return _drawPile.RemoveFirstDTO(card);
        }

        public CardDeck GetDrawPile()
        {
            return _drawPile;
        }

        public CardDeck GetDiscardPile()
        {
            return _discardPile;
        }

        public void UpdateCardDecksFromDb(List<CardDto> drawPile, List<CardDto> discardPile)
        {
            var newDrawPile = drawPile.Select(RemoveCardByDTO).ToList();
            _drawPile.AddMany(newDrawPile);
            var newDiscardPile = discardPile.Select(RemoveCardByDTO).ToList();
            _discardPile.AddMany(newDiscardPile);
        }
    }
}
