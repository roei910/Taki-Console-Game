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
        
        //TODO check if needed
        public CardDeck(CardDeck other) : this(other._cards.ToList(), other._serviceProvider){ }


        public Card GetFirst()
        {
            return _cards.First();
        }

        public void RemoveFirst()
        {
            _cards.RemoveFirst();
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

            //TODO: refactor
            //var cards = _cards.ToList();
            //_cards = new(cards.Select(_ =>
            //{
            //    Card card = _cards.ElementAt(random.Next(_cards.Count));
            //    _cards.Remove(card);
            //    return card;
            //}).ToList());
            _cards = new(_cards.OrderBy(val => Guid.NewGuid().ToString()).ToList());
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

        public void AddMany(List<Card> playerCards)
        {
            if(playerCards.Count > 0)
                _cards.AddLast(playerCards.First());
        }
    }
}
