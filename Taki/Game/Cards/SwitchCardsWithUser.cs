using System.Drawing;
using Taki.Game.Deck;
using Taki.Game.Messages;
using Taki.Game.Players;

namespace Taki.Game.Cards
{
    internal class SwitchCardsWithUser : SwitchCardsWithDirection
    {
        public SwitchCardsWithUser(IUserCommunicator userCommunicator) : 
            base(userCommunicator) { }

        public override void Play(Card topDiscard, ICardDecksHolder cardDecksHolder, IPlayersHolder playersHolder)
        {
            Player currentPlayer = playersHolder.CurrentPlayer;
            Player playerToSwitch = currentPlayer.PickOtherPlayer(playersHolder);

            (currentPlayer.PlayerCards, playerToSwitch.PlayerCards) = (playerToSwitch.PlayerCards, currentPlayer.PlayerCards);

            _userCommunicator.SendErrorMessage($"Player: {currentPlayer.Name} used switch cards with Player: {playerToSwitch.Name}!\n");

            playersHolder.NextPlayer();
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

        public override string ToString()
        {
            if (prevCard == null)
                return "Switch Cards With User";
            return $"Switch Cards With User, previous {prevCard}";
        }
    }
}
