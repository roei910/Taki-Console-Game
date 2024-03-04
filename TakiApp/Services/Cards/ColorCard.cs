using TakiApp.Models;

namespace TakiApp.Services.Cards
{
    public abstract class ColorCard : ICardService
    {
        public virtual bool CanStackOnOther(Card topDiscard, Card otherCard)
        {
            if (topDiscard.CardColor.Equals(otherCard.CardColor))
                return true;

            if (topDiscard.Type == otherCard.Type)
                return true;

            return false;
        }

        public abstract List<Card> GenerateCardsForDeck();
    }
}
