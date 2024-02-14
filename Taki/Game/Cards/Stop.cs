using Microsoft.Extensions.DependencyInjection;
using System.Drawing;
using Taki.Game.Handlers;
using Taki.Game.Messages;
using Taki.Game.Players;

namespace Taki.Game.Cards
{
    internal class Stop : ColorCard
    {
        public Stop(Color color) : base(color) { }

        public override bool IsStackableWith(Card other)
        {
            return base.IsStackableWith(other) || other is Stop;
        }

        public override void Play(Card topDiscard, IPlayersHandler playersHandler, 
            IServiceProvider serviceProvider)
        {
            IUserCommunicator userCommunicator = serviceProvider.GetRequiredService<IUserCommunicator>();
            Player currentPlayer = playersHandler.GetCurrentPlayer();
            playersHandler.NextPlayer();

            Player nextPlayer = playersHandler.GetCurrentPlayer();
            userCommunicator.SendErrorMessage(
                $"{nextPlayer.GetName()} was stopped by " +
                $"{currentPlayer.GetName()}\n");

            base.Play(topDiscard, playersHandler, serviceProvider);
        }

        public override string ToString()
        {
            return $"Stop {base.ToString()}";
        }
    }
}
