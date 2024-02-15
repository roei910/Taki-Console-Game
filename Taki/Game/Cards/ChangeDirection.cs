using System.Drawing;
using Taki.Game.Players;

namespace Taki.Game.Cards
{
    internal class ChangeDirection : ColorCard
    {
        public ChangeDirection(Color color) : base(color) { }

        public override bool IsStackableWith(Card other)
        {
            return base.IsStackableWith(other) || other is ChangeDirection;
        }

        public override void Play(Card topDiscard, IPlayersHolder playersHolder, IServiceProvider serviceProvider)
        {
            playersHolder.ChangeDirection();
            base.Play(topDiscard, playersHolder, serviceProvider);
        }

        public override string ToString()
        {
            return $"ChangeDirection, {base.ToString()}";
        }
    }
}
