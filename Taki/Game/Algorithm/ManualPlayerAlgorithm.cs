using System.Drawing;
using Taki.Game.Cards;
using Taki.Game.Messages;

namespace Taki.Game.Algorithm
{
    internal class ManualPlayerAlgorithm : IPlayerAlgorithm
    {
        public override string ToString()
        {
            return "Manual Player Algo";
        }

        public Card? ChooseCard(Func<Card, bool> isSimilarTo, List<Card> playerCards, IUserCommunicator userCommunicator)
        {
            playerCards = OrderPlayerCardByColor(playerCards);
            var playerCardsString = playerCards.Select((card, i) => $"{i}. {card}").ToList();
            userCommunicator.SendMessageToUser(string.Join("\n", playerCardsString));
            userCommunicator.SendAlertMessage($"Please choose one of your cards by index, " +
                $"-1 to draw a card");

            return ChooseValidCard(userCommunicator, playerCards, isSimilarTo);
        }

        public Color ChooseColor(List<Card> playerCards, IUserCommunicator userCommunicator)
        {
            return userCommunicator.GetColorFromUserEnum<CardColorsEnum>();
        }

        private Card? ChooseValidCard(IUserCommunicator userCommunicator, 
            List<Card> playerCards, Func<Card, bool> isSimilarTo)
        {
            if (!int.TryParse(userCommunicator.GetMessageFromUser(), out int index)
                || !IsValidIndex(index, playerCards.Count))
            {
                userCommunicator.SendMessageToUser("please choose again the index of the card");
                return ChooseValidCard(userCommunicator, playerCards, isSimilarTo);
            }

            if (index == -1)
                return null;

            Card playerCard = playerCards.ElementAt(index);
            if (!isSimilarTo(playerCard))
            {
                userCommunicator.SendMessageToUser("card does not meet the stacking rules");
                return ChooseValidCard(userCommunicator, playerCards, isSimilarTo);
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
