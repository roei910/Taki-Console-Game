using System.Drawing;
using Taki.Game.Handlers;
using Taki.Game.Messages;

namespace Taki.Game.Cards
{
    internal class ChangeColor : Card
    {
        static readonly Color DEFAULT_COLOR = Color.Empty;
        Color color = DEFAULT_COLOR;

        public override bool IsStackableWith(Card other)
        {
            if (color == DEFAULT_COLOR)
                return true;
            return other.IsStackableWith(new NumberCard(0, color));
        }

        public override void Play(IPlayersHandler playersHandler, ICardsHandler cardsHandler, IUserCommunicator userCommunicator)
        {
            while (!ColorCard.Colors.Contains(color))
                color = playersHandler.GetCurrentPlayer().ChooseColor(playersHandler, userCommunicator);
            
            base.Play(playersHandler, cardsHandler, userCommunicator);
        }

        public override void FinishPlay()
        {
            color = DEFAULT_COLOR;
        }

        public override string ToString()
        {
            return $"ChangeColor {color}";
        }
    }
}
