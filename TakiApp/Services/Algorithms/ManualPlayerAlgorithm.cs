using System.Drawing;
using TakiApp.Services.Cards;
using TakiApp.Shared.Interfaces;
using TakiApp.Shared.Models;

namespace Taki.Models.Algorithm
{
    public class ManualPlayerAlgorithm : IManualPlayerAlgorithm
    {
        private readonly IUserCommunicator _userCommunicator;

        public ManualPlayerAlgorithm(IUserCommunicator userCommunicator)
        {
            _userCommunicator = userCommunicator;
        }

        public Card? ChooseCard(Func<Card, bool> isSimilarTo, List<Card> playerCards, string? elseMessage = null)
        {
            playerCards = OrderPlayerCardByColor(playerCards);
            _userCommunicator.SendAlertMessage("printing your current hand:");

            //TODO: use the user communicator function with list, and an else option?
            var playerCardsString = playerCards.Select((card, i) => $"{i}. {card}").ToList();
            _userCommunicator.SendMessageToUser(string.Join("\n", playerCardsString));

            string message = $"Please choose one of your cards by index, " + (elseMessage is null ? $"-1 to draw a card" : elseMessage);
            _userCommunicator.SendAlertMessage(message);

            return ChooseValidCard(playerCards, isSimilarTo);
        }

        public Color ChooseColor(List<Card> playerCards)
        {
            return _userCommunicator.UserPickItemFromList(ColorCard.Colors, printPrompt: true);
        }

        public Player ChoosePlayer(List<Player> players)
        {
            var messages = players.Select((player, i) =>
                $"{i}. {player.Name}").ToList();

            _userCommunicator.SendAlertMessage("Please choose one of the players by index:");
            int index = _userCommunicator.GetNumberFromUser(string.Join("\n", messages));

            return players[index];
        }

        private Card? ChooseValidCard(List<Card> playerCards, Func<Card, bool> isSimilarTo)
        {
            if (!int.TryParse(_userCommunicator.GetMessageFromUser(), out int index)
                || !IsValidIndex(index, playerCards.Count))
            {
                _userCommunicator.SendMessageToUser("please choose again the index of the card");
                return ChooseValidCard(playerCards, isSimilarTo);
            }

            if (index == -1)
                return null;

            Card playerCard = playerCards[index];
            if (!isSimilarTo(playerCard))
            {
                _userCommunicator.SendErrorMessage("card does not meet the stacking rules");
                return ChooseValidCard(playerCards, isSimilarTo);
            }

            return playerCard;
        }

        private List<Card> OrderPlayerCardByColor(List<Card> playerCards)
        {
            return playerCards.OrderBy(card => card.CardColor).ToList();
        }

        private bool IsValidIndex(int index, int maxCards)
        {
            return index >= -1 && index < maxCards;
        }
    }
}
