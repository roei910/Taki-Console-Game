using Taki.Models.Deck;
using Taki.Shared.Abstract;
using Taki.Shared.Models.Dto;

namespace Taki.Shared.Interfaces
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