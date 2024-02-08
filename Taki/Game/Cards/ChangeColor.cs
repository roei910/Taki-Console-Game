
using System.Drawing;
using Taki.Game.Handlers;

namespace Taki.Game.Cards
{
    internal class ChangeColor : Card
    {
        static readonly Color DEFAULT_COLOR = Color.Empty;
        Color color = DEFAULT_COLOR;

        public override bool IsSimilarTo(Card other)
        {
            if (color == DEFAULT_COLOR)
                return true;
            return other.IsSimilarTo(new NumberCard(0, color));
        }

        public override void Play(Card topDiscard, GameHandlers gameHandlers)
        {
            color = gameHandlers.GetUtilities().GetColorFromUserEnum<CardColorsEnum>();

            while (!ColorCard.Colors.Contains(color))
                color = gameHandlers.GetUtilities().GetColorFromUserEnum<CardColorsEnum>();

            gameHandlers.GetPlayersHandler().NextPlayer();
        }

        public override void FinishPlay()
        {
            color = DEFAULT_COLOR;
        }

        public override string ToString()
        {
            return "ChangeColor";
        }
    }
}
