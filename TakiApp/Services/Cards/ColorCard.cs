using System.Drawing;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Services.Cards
{
    public abstract class ColorCard : CardService
    {
        public static readonly Color DEFAULT_COLOR = Color.Empty;
        public static List<Color> Colors = [Color.Green, Color.Red, Color.Yellow, Color.Blue];

        protected ColorCard(IDiscardPileRepository discardPileRepository, IPlayersRepository playersRepository) : 
            base(discardPileRepository, playersRepository) { }

        public override bool CanStackOtherOnThis(Card topDiscard, Card otherCard)
        {
            if (topDiscard.CardColor == DEFAULT_COLOR.ToString())
                return true;

            if (topDiscard.CardColor == otherCard.CardColor)
                return true;

            return base.CanStackOtherOnThis(topDiscard, otherCard);
        }
    }
}
