using System.Drawing;
using Taki.Game.Handlers;

namespace Taki.Game.Cards
{
    internal class SuperTaki : Card
    {
        private TakiCard? takiInstance;

        public override void FinishNoPlay()
        {
            takiInstance?.FinishNoPlay();
        }

        public override void FinishPlay()
        {
            takiInstance?.FinishPlay();
        }

        public override void Play(GameHandlers gameHandlers)
        {
            Color color = Color.Empty;
            while (!ColorCard.Colors.Contains(color))
                color = gameHandlers.GetPlayersHandler().CurrentPlayer.ChooseColor(gameHandlers);

            takiInstance = new TakiCard(color);
            takiInstance.Play(gameHandlers);
        }

        public override bool IsSimilarTo(Card other)
        {
            if (takiInstance is null)
                return true;
            return takiInstance.IsSimilarTo(other);
        }

        public override string ToString()
        {
            return "SUPER-TAKI";
        }
    }
}
