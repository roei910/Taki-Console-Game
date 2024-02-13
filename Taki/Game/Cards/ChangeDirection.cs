using System.Drawing;
using Taki.Game.Handlers;

namespace Taki.Game.Cards
{
    internal class ChangeDirection : ColorCard
    {
        public ChangeDirection(Color color) : base(color) { }

        public override bool IsSimilarTo(Card other)
        {
            return base.IsSimilarTo(other) || other is ChangeDirection;
        }

        public override void Play(GameHandlers gameHandlers)
        {
            gameHandlers.GetPlayersHandler().ChangeDirection();
            base.Play(gameHandlers);
        }

        public override string ToString()
        {
            return $"ChangeDirection, {base.ToString()}";
        }
    }
}
