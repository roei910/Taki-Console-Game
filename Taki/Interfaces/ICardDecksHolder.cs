using Taki.Dto;
using Taki.Models.Cards;
using Taki.Models.Deck;

namespace Taki.Interfaces
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
        CardDeck GetDrawPile();
        CardDeck GetDiscardPile();
        void UpdateCardDecksFromDb(List<CardDto> drawPile, List<CardDto> discardPile);
    }
}