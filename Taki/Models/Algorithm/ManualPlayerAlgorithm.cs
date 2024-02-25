using System.Drawing;
using Taki.Interfaces;
using Taki.Models.Cards;
using Taki.Models.Players;

namespace Taki.Models.Algorithm
{
    internal class ManualPlayerAlgorithm : IPlayerAlgorithm
    {
        private readonly IUserCommunicator _userCommunicator;

        public ManualPlayerAlgorithm(IUserCommunicator userCommunicator)
        {
            _userCommunicator = userCommunicator;
        }

        public override string ToString()
        {
            return "Manual Player Algo";
        }

        public Card? ChooseCard(Func<Card, bool> isSimilarTo, List<Card> playerCards, string? elseMessage = null)
        {
            playerCards = OrderPlayerCardByColor(playerCards);
            _userCommunicator.SendAlertMessage("printing your current hand:");
            var playerCardsString = playerCards.Select((card, i) => $"{i}. {card}").ToList();
            _userCommunicator.SendMessageToUser(string.Join("\n", playerCardsString));

            string message = $"Please choose one of your cards by index, " + (elseMessage is null ? $"-1 to draw a card" : elseMessage);
            _userCommunicator.SendAlertMessage(message);

            return ChooseValidCard(playerCards, isSimilarTo);
        }

        public Color ChooseColor(List<Card> playerCards)
        {
            return _userCommunicator.GetColorFromUserEnum<CardColorsEnum>();
        }

        public Player ChoosePlayer(Player currentPlayer, IPlayersHolder playersHolder)
        {
            var players = playersHolder.Players.Where(p => p.Id != currentPlayer.Id).ToList();
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

            Card playerCard = playerCards.ElementAt(index);
            if (!isSimilarTo(playerCard))
            {
                _userCommunicator.SendErrorMessage("card does not meet the stacking rules");
                return ChooseValidCard(playerCards, isSimilarTo);
            }

            return playerCard;
        }

        private List<Card> OrderPlayerCardByColor(List<Card> playerCards)
        {
            return playerCards.OrderBy(card =>
            {
                if (card is ColorCard colorCard)
                    return colorCard.GetColor().ToString();
                return Color.Empty.ToString();
            }).ToList();
        }

        private bool IsValidIndex(int index, int maxCards)
        {
            return index >= -1 && index < maxCards;
        }
    }
}
