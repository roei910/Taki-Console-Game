using Taki.Game.Cards;

namespace Taki.Game.Deck
{
    internal class CardDeck
    {
        private LinkedList<Card> _cards;

        public CardDeck(List<Card> cards)
        {
            _cards = new(cards);
        }

        public CardDeck() : this(new List<Card>()) { }

        public CardDeck(CardDeck other) : this(other._cards.ToList()){ }


        public Card GetFirst()
        {
            return _cards.First();
        }

        public void RemoveFirst()
        {
            _cards.RemoveFirst();
        }

        public Card GetLast()
        {
            return _cards.Last();
        }

        public void RemoveLast()
        {
            _cards.RemoveLast();
        }

        public void AddFirst(Card card)
        {
            _cards.AddFirst(card);
        }

        public void AddLast(Card card)
        {
            _cards.AddLast(card);
        }

        public int Count()
        {
            return _cards.Count;
        }

        public void ShuffleDeck()
        {
            Random random = new();
            var cards = _cards.ToList();
            _cards = new(cards.Select(_ =>
            {
                Card card = _cards.ElementAt(random.Next(_cards.Count));
                _cards.Remove(card);
                return card;
            }));
        }

        public void CombineDeckToThis(CardDeck cardDeck)
        {
            List<Card> cards = [.. _cards.ToList(), .. cardDeck._cards.ToList()];
            cardDeck._cards = [];
            _cards = new(cards);
        }

        public override string ToString()
        {
            return $"CardDeck: {_cards.Count} no of cards";
        }
    }
}
