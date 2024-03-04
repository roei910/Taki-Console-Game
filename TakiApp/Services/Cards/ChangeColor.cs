using System.Drawing;
using TakiApp.Models;

namespace TakiApp.Services.Cards
{
    public class ChangeColor : ICardService
    {
        public bool CanStackOnOther(Card topDiscard, Card otherCard)
        {
            if (topDiscard.CardColor == Color.Empty.Name)
                return true;

            return topDiscard.CardColor == otherCard.CardColor;
        }

        public List<Card> GenerateCardsForDeck()
        {
            return Enumerable.Range(0, 2)
                .Select(j => new Card(typeof(ChangeColor).ToString(), Color.Empty.Name)).ToList();
        }
    }
}
