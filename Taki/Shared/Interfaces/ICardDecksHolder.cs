using Taki.Models.Deck;
using Taki.Shared.Abstract;
using Taki.Shared.Models.Dto;

namespace Taki.Shared.Interfaces
{
    internal interface ICardDecksHolder
    {
        Card GetTopDiscard();
        void AddDiscardCard(Card card);
        void ResetCards();
        void ResetCards(List<Card> playerCards);
        Card? DrawCard();
        void DrawFirstCard();
        int CountAllCards();
        Card RemoveCardByDTO(CardDto card);
        CardDeck GetDrawCardDeck();
        CardDeck GetDiscardCardDeck();
        void UpdateTopDiscardInDB();
    }
}