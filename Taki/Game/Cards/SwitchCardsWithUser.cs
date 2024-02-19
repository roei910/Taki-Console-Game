using System.Drawing;
using Taki.Game.Deck;
using Taki.Game.Messages;
using Taki.Game.Players;

namespace Taki.Game.Cards
{
    internal class SwitchCardsWithUser : Card
    {
        public SwitchCardsWithUser(IUserCommunicator userCommunicator) : 
            base(userCommunicator) { }

        public override void Play(Card topDiscard, ICardDecksHolder cardDecksHolder, IPlayersHolder playersHolder)
        {
            Player currentPlayer = playersHolder.CurrentPlayer;
            Player playerToSwitch = GetPlayerFromUser(playersHolder);

            (currentPlayer.PlayerCards, playerToSwitch.PlayerCards) = (currentPlayer.PlayerCards, playerToSwitch.PlayerCards);

            _userCommunicator.SendErrorMessage($"User used switch cards with Player: {playerToSwitch.Name}!\n");

            base.Play(topDiscard, cardDecksHolder, playersHolder);
        }

        //TODO: move function inside algorithm
        private Player GetPlayerFromUser(IPlayersHolder playersHolder)
        {
            _userCommunicator.SendAlertMessage("Please choose one of the players by index:");
            var players = playersHolder.Players.Select((player, i) =>
                $"{i}. {player.Name}").ToList();

            int index = _userCommunicator.GetNumberFromUser(string.Join("\n", players));

            return playersHolder.Players.ElementAt(index);
        }

        public override void PrintCard()
        {
            string[] numberInArray = [
                "**********",
                "* SWITCH *",
                "*        *",
                "* USER   *",
                "*        *",
                "* CARDS  *",
                "**********"];

            _userCommunicator.SendColorMessageToUser(Color.White, string.Join("\n", numberInArray));
        }

        public override bool IsStackableWith(Card other)
        {
            return true;
        }

        public override string ToString()
        {
            return "Switch Cards With User";
        }
    }
}
