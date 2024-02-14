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

        public override void Play(IPlayersHandler playersHandler, 
            ICardsHandler cardsHandler, IUserCommunicator userCommunicator)
        {
            Player currentPlayer = playersHandler.GetCurrentPlayer();
            playersHandler.NextPlayer();

            Player nextPlayer = playersHandler.GetCurrentPlayer();
            userCommunicator.SendErrorMessage(
                $"{nextPlayer.GetName()} was stopped by " +
                $"{currentPlayer.GetName()}\n");

            base.Play(playersHandler, cardsHandler, userCommunicator);
        }

        public override string ToString()
        {
            return $"Stop {base.ToString()}";
        }
    }
}
