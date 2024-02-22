using Taki.Game.Dto;
using Taki.Game.Models.Cards;
using Taki.Game.Models.Deck;

namespace Taki.Game.Interfaces
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