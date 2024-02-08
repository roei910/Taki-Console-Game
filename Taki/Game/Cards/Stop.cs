using System.Drawing;
using Taki.Game.Handlers;

namespace Taki.Game.Cards
{
    internal class Stop : ColorCard
    {
        public Stop(Color color) : base(color) { }

        public override bool IsSimilarTo(Card other)
        {
            return base.IsSimilarTo(other) || other is Stop;
        }

        public override void Play(Card topDiscard, GameHandlers gameHandlers)
        {
            gameHandlers.GetPlayersHandler().NextPlayer();
            gameHandlers.GetPlayersHandler().NextPlayer();
        }

        public override string ToString()
        {
            return $"Stop {base.ToString()}";
        }
    }
}
