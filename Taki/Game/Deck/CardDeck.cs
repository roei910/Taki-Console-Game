using Microsoft.Extensions.DependencyInjection;
using Taki.Game.Cards;

namespace Taki.Game.Deck
{
    internal class CardDeck : ICardDeck
    {
        private readonly IServiceProvider _serviceProvider;
        private LinkedList<Card> _cards;

        public CardDeck(List<Card> cards, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _cards = new(cards);
        }

        public CardDeck(IServiceProvider serviceProvider) : this(new List<Card>(), serviceProvider) { }
        
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
            var random = _serviceProvider.GetRequiredService<Random>();

            _cards = new(_cards.OrderBy(val => random.Next(_cards.Count)));
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
