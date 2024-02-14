using Taki.Game.Cards;

namespace Taki.Game.Handlers
{
    internal interface ICardsHandler
    {
        Card GetTopDiscard();
        void AddDiscardCard(Card card);
        void ResetCards();
        void ResetCards(List<Card> playerCards);
        Card? DrawCard();
        void DrawFirstCard();
    }
}