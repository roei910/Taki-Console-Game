using Taki.Dto;
using Taki.Models.Cards;
using Taki.Models.Deck;

namespace Taki.Interfaces
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