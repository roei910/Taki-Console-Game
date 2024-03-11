using System.Drawing;
using TakiApp.Shared.Interfaces;
using TakiApp.Shared.Models;

namespace TakiApp.Services.Cards
{
    public abstract class ColorCard : CardService
    {
        public static readonly Color DEFAULT_COLOR = Color.Empty;
        public static readonly List<Color> Colors = [Color.Green, Color.Red, Color.Yellow, Color.Blue];

        protected ColorCard(IDiscardPileRepository discardPileRepository, IPlayersRepository playersRepository, 
            IUserCommunicator userCommunicator) : 
            base(discardPileRepository, playersRepository, userCommunicator) { }

        public override bool CanStackOtherOnThis(Card topDiscard, Card otherCard, ICardPlayService cardPlayService)
        {
            if (topDiscard.CardColor == DEFAULT_COLOR.Name)
                return true;

            if (topDiscard.CardColor == otherCard.CardColor)
                return true;

            return base.CanStackOtherOnThis(topDiscard, otherCard, cardPlayService);
        }
    }
}
