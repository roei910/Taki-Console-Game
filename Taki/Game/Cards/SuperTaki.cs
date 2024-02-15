using Microsoft.Extensions.DependencyInjection;
using System.Drawing;
using Taki.Game.Messages;
using Taki.Game.Players;

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

        public override void Play(Card topDiscard, IPlayersHolder playersHolder, 
            IServiceProvider serviceProvider)
        {
            IUserCommunicator userCommunicator = serviceProvider.GetRequiredService<IUserCommunicator>();
            Color color = Color.Empty;

            while (!ColorCard.Colors.Contains(color))
                color = playersHolder.CurrentPlayer.ChooseColor();

            takiInstance = new TakiCard(color);
            takiInstance.Play(topDiscard, playersHolder, serviceProvider);
        }

        public override bool IsStackableWith(Card other)
        {
            if (takiInstance is null)
                return true;
            return takiInstance.IsStackableWith(other);
        }

        public override string ToString()
        {
            return "SUPER-TAKI";
        }
    }
}
