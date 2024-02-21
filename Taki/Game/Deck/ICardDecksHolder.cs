using Taki.Game.Cards;

namespace Taki.Game.Deck
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
        Card RemoveCardByDTO(CardDTO card);
        CardDeck GetDrawPile();
        CardDeck GetDiscardPile();
        void UpdateCardsFromDB(List<CardDTO> drawPile, List<CardDTO> discardPile);
    }
}