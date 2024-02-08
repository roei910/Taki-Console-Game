using System.Drawing;
using Taki.Game.GameRules;
using Taki.Game.Handlers;

namespace Taki.Game.Cards
{
    internal class Plus2 : ColorCard
    {
        private bool IsOnlyPlus2Allowed = false;
        private int countPlus2 = 0;

        public Plus2(Color color) : base(color) { }

        public override bool IsSimilarTo(Card other)
        {
            if(IsOnlyPlus2Allowed)
                return other is Plus2;
            return base.IsSimilarTo(other) || other is Plus2;
        }

        public override int CardsToDraw()
        {
            return countPlus2 * 2;
        }

        public override void Play(Card topDiscard, GameHandlers gameHandlers)
        {
            if (topDiscard is Plus2 card)
            {
                countPlus2 = card.countPlus2;
                card.FinishNoPlay();
            }
            IsOnlyPlus2Allowed = true;
            countPlus2++;
            gameHandlers.GetPlayersHandler().NextPlayer();
        }

        public override string ToString()
        {
            return $"Plus2 {base.ToString()}";
        }

        public override void FinishNoPlay()
        {
            IsOnlyPlus2Allowed = false;
            countPlus2 = 0;
        }
    }
}
