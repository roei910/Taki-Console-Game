using System.Drawing;
using Taki.Game.Handlers;

namespace Taki.Game.Cards
{
    internal class SuperTaki : Card
    {
        private TakiCard? takiInstance;
        private ChangeColor? changeColorInstance;

        public override void FinishNoPlay()
        {
            changeColorInstance?.FinishNoPlay();
            takiInstance?.FinishNoPlay();
        }

        public override void FinishPlay()
        {
            changeColorInstance?.FinishPlay();
            takiInstance?.FinishPlay();
        }

        public override void Play(Card topDiscard, GameHandlers gameHandlers)
        {
            changeColorInstance = new ChangeColor();
            changeColorInstance.Play(topDiscard, gameHandlers);

            takiInstance = new TakiCard(Color.Empty);
            takiInstance.Play(topDiscard, gameHandlers);
        }

        public override bool IsSimilarTo(Card other)
        {
            if (changeColorInstance is null)
                return true;
            return changeColorInstance.IsSimilarTo(other);
        }

        public override string ToString()
        {
            return "SUPER-TAKI";
        }
    }
}
