using System.Drawing;
using Taki.Game.Handlers;

namespace Taki.Game.Cards
{
    internal class TakiCard : ColorCard
    {
        //TODO: other cards play interupt the taki
        private bool IsTakiOpen = false;
        public TakiCard(Color color) : base(color) { }

        public override void FinishNoPlay()
        {
            IsTakiOpen = false;
        }

        public override bool IsSimilarTo(Card other)
        {
            return base.IsSimilarTo(other) || other is TakiCard;
        }

        public override void Play(Card topDiscard, GameHandlers gameHandlers)
        {
            IsTakiOpen = true;
            gameHandlers.GetPlayersHandler().NextPlayer();
        }

        public override string ToString()
        {
            return $"TAKI, {base.ToString()}";
        }
    }
}
