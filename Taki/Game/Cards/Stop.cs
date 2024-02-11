using System.Drawing;
using Taki.Game.Handlers;
using Taki.Game.Players;

namespace Taki.Game.Cards
{
    internal class Stop : ColorCard
    {
        public Stop(Color color) : base(color) { }

        public override bool IsSimilarTo(Card other)
        {
            return base.IsSimilarTo(other) || other is Stop;
        }

        public override void Play(GameHandlers gameHandlers)
        {
            Player currentPlayer = gameHandlers.GetPlayersHandler().CurrentPlayer;
            gameHandlers.GetPlayersHandler().NextPlayer();

            Player nextPlayer = gameHandlers.GetPlayersHandler().CurrentPlayer;
            gameHandlers.GetMessageHandler().SendErrorMessage(
                $"{nextPlayer.GetName()} was stopped by " +
                $"{currentPlayer.GetName()}\n");

            gameHandlers.GetPlayersHandler().NextPlayer();
        }

        public override string ToString()
        {
            return $"Stop {base.ToString()}";
        }
    }
}
