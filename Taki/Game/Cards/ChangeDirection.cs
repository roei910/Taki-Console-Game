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

        public override void Play(Card topDiscard, GameHandlers gameHandlers)
        {
            gameHandlers.GetPlayersHandler().ChangeDirection();
            gameHandlers.GetPlayersHandler().NextPlayer();
        }

        public override string ToString()
        {
            return $"ChangeDirection, {base.ToString()}";
        }
    }
}
