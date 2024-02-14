using Microsoft.Extensions.DependencyInjection;
using System.Drawing;
using Taki.Game.Handlers;
using Taki.Game.Messages;

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

        public override void Play(Card topDiscard, IPlayersHandler playersHandler, 
            IServiceProvider serviceProvider)
        {
            IUserCommunicator userCommunicator = serviceProvider.GetRequiredService<IUserCommunicator>();
            Color color = Color.Empty;

            while (!ColorCard.Colors.Contains(color))
                color = playersHandler.GetCurrentPlayer().ChooseColor(playersHandler, userCommunicator);

            takiInstance = new TakiCard(color);
            takiInstance.Play(topDiscard, playersHandler, serviceProvider);
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
