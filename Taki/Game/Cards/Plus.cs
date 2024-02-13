using System.Drawing;
using Taki.Game.Handlers;

namespace Taki.Game.Cards
{
    internal class Plus : ColorCard
    {
        public Plus(Color color) : base(color) { }

        public override bool IsSimilarTo(Card other)
        {
            return base.IsSimilarTo(other) || other is Plus;
        }

        public override void Play(GameHandlers gameHandlers)
        {
            ;
        }

        public override string ToString()
        {
            return $"Plus {base.ToString()}";
        }
    }
}
