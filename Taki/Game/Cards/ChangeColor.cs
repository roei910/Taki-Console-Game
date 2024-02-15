using Microsoft.Extensions.DependencyInjection;
using System.Drawing;
using Taki.Game.Messages;
using Taki.Game.Players;

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

        public override void Play(Card topDiscard, IPlayersHolder playersHolder, IServiceProvider serviceProvider)
        {
            while (!ColorCard.Colors.Contains(color))
                color = playersHolder.CurrentPlayer.ChooseColor();
            
            base.Play(topDiscard, playersHolder, serviceProvider);
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
