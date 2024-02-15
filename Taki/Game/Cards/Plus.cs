using System.Drawing;
using Taki.Game.Players;

namespace Taki.Game.Cards
{
    internal class Plus : ColorCard
    {
        public Plus(Color color) : base(color) { }

        public override bool IsStackableWith(Card other)
        {
            return base.IsStackableWith(other) || other is Plus;
        }

        public override void Play(Card topDiscard, IPlayersHolder playersHolder, 
            IServiceProvider serviceProvider) { }

        public override string ToString()
        {
            return $"Plus {base.ToString()}";
        }
    }
}
