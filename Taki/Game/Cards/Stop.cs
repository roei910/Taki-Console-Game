using System.Drawing;
using Taki.Game.Deck;
using Taki.Game.Messages;
using Taki.Game.Players;

namespace Taki.Game.Cards
{
    internal class Stop : ColorCard
    {
        public Stop(Color color, IUserCommunicator userCommunicator) : 
            base(color, userCommunicator) { }

        public override bool IsStackableWith(Card other)
        {
            return base.IsStackableWith(other) || other is Stop;
        }

        public override void Play(Card topDiscard, ICardDecksHolder cardDecksHolder, IPlayersHolder playersHolder)
        {
            Player currentPlayer = playersHolder.CurrentPlayer;
            playersHolder.NextPlayer();

            Player nextPlayer = playersHolder.CurrentPlayer;
            _userCommunicator.SendErrorMessage(
                $"{nextPlayer.Name} was stopped by " +
                $"{currentPlayer.Name}\n");

            base.Play(topDiscard, cardDecksHolder, playersHolder);
        }

        public override void PrintCard()
        {
            string[] numberInArray = [
                "**********",
                "*  STOP  *",
                "*        *",
                "*        *",
                "*        *",
                "*        *",
                "**********"];

            _userCommunicator.SendColorMessageToUser(_color, string.Join("\n", numberInArray));
        }

        public override string ToString()
        {
            return $"Stop {base.ToString()}";
        }
    }
}
