using Taki.Game.Cards;

namespace Taki.Game.Deck
{
    internal interface ICardDeck
    {
        Card PopFirst();
        Card GetFirst();
        void AddFirst(Card card);
        int Count();
        void ShuffleDeck();
        void CombineFromDeck(CardDeck other);
        void AddMany(List<Card> playerCards);
        Card RemoveFirstDTO(CardDTO card);
        List<Card> GetAllCards();
    }
}