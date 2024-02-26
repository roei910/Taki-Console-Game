using Taki.Models.Players;
using Taki.Shared.Abstract;
using Taki.Shared.Interfaces;

namespace Taki.Models.Cards
{
    internal class SwitchCardsWithUser : SwitchCardsWithDirection
    {
        public SwitchCardsWithUser(IUserCommunicator userCommunicator) :
            base(userCommunicator)
        { }

        public override void Play(Card topDiscard, ICardDecksHolder cardDecksHolder, IPlayersHolder playersHolder)
        {
            prevCard = topDiscard is SwitchCardsWithDirection card ? card.prevCard : topDiscard;

            Player currentPlayer = playersHolder.CurrentPlayer;
            Player playerToSwitch = currentPlayer.PickOtherPlayer(playersHolder);

            (currentPlayer.PlayerCards, playerToSwitch.PlayerCards) = (playerToSwitch.PlayerCards, currentPlayer.PlayerCards);

            _userCommunicator.SendErrorMessage($"Player: {currentPlayer.Name} used switch cards with Player: {playerToSwitch.Name}!\n");

            playersHolder.NextPlayer();
        }

        public override string[] GetStringArray()
        {
            return [
                "**********",
                "* SWITCH *",
                "*        *",
                "* USER   *",
                "*        *",
                "* CARDS  *",
                "**********"];
        }

        public override string ToString()
        {
            if (prevCard == null)
                return "Switch Cards With User";
            return $"Switch Cards With User, previous {prevCard}";
        }
    }
}
