using System.Drawing;
using Taki.Models.Players;
using Taki.Shared.Abstract;
using Taki.Shared.Interfaces;

namespace Taki.Models.Cards
{
    internal class Stop : ColorCard
    {
        public Stop(Color color, IUserCommunicator userCommunicator) :
            base(color, userCommunicator)
        { }

        public override string[] GetStringArray()
        {
            return [
                "**********",
                "*  STOP  *",
                "*        *",
                "*        *",
                "*        *",
                "*        *",
                "**********"];
        }

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

        public override string ToString()
        {
            return $"Stop {base.ToString()}";
        }
    }
}
