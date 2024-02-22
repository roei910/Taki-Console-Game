using Taki.Game.Cards;
using Taki.Game.Cards.DTOs;

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
        Card RemoveFirstDTO(CardDto card);
        List<Card> GetAllCards();
    }
}