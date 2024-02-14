using Taki.Game.Cards;

namespace Taki.Game.Deck
{
    internal interface ICardDeck
    {
        Card GetFirst();
        void RemoveFirst();
        void AddFirst(Card card);
        int Count();
        void ShuffleDeck();
        void CombineDeckToThis(CardDeck cardDeck);
        void AddMany(List<Card> playerCards);
    }
}