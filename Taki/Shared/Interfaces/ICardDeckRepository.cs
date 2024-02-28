using Taki.Models.Deck;
using Taki.Shared.Abstract;
using Taki.Shared.Models.Dto;

namespace Taki.Shared.Interfaces
{
    internal interface ICardDeckRepository
    {
        void AddDiscardCard(Card card);
        void DeleteAll();
        void DrawCard(Card card);
        List<CardDto> GetDiscardCards();
        List<CardDto> GetDrawCards();
        void UpdateAllCards(CardDeck discardPile, CardDeck drawPile);
        void UpdateTopDiscard(CardDto card);
    }
}