using System.Drawing;
using Taki.Game.Handlers;

namespace Taki.Game.Cards
{
    internal class ChangeDirection : ColorCard
    {
        public ChangeDirection(Color color) : base(color) { }

        public override bool IsStackableWith(Card other)
        {
            return base.IsStackableWith(other) || other is ChangeDirection;
        }

        public override void Play(Card topDiscard, IPlayersHandler playersHandler, IServiceProvider serviceProvider)
        {
            playersHandler.ChangeDirection();
            base.Play(topDiscard, playersHandler, serviceProvider);
        }

        public override string ToString()
        {
            return $"ChangeDirection, {base.ToString()}";
        }
    }
}
