using Microsoft.Extensions.DependencyInjection;
using Taki.Game.Cards;

namespace Taki.Game.Deck
{
    internal class CardDeck : ICardDeck
    {
        private LinkedList<Card> _cards;
        private readonly Random _random;

        public CardDeck(List<Card> cards, Random random)
        {
            _cards = new(cards);
            _random = random;
        }

        public CardDeck(Random random) : this(new List<Card>(), random) { }
        
        public Card GetFirst()
        {
            return _cards.First();
        }

        public void AddFirst(Card card)
        {
            _cards.AddFirst(card);
        }

        public int Count()
        {
            return _cards.Count;
        }

        public void ShuffleDeck()
        {
            _cards = new(_cards.OrderBy(val => _random.Next(_cards.Count)));
        }

        public void CombineFromDeck(CardDeck other)
        {
            List<Card> cards = [.. _cards.ToList(), .. other._cards.ToList()];
            other._cards = [];
            _cards = new(cards);
        }

        public override string ToString()
        {
            return $"CardDeck: {_cards.Count} no of cards";
        }

        public void AddMany(List<Card> playerCards)
        {
            if(playerCards.Count > 0)
                _cards.AddLast(playerCards.First());
        }

        public Card PopFirst()
        {
            Card card = _cards.First();
            _cards.RemoveFirst();
            return card;
        }
    }
}
