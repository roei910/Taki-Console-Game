using Microsoft.Extensions.DependencyInjection;
using System.Drawing;
using Taki.Game.Cards;
using Taki.Game.Handlers;
using Taki.Game.Messages;
using Taki.Game.Players;

namespace Taki.Game.Algorithm
{
    internal class ManualPlayerAlgorithm : IPlayerAlgorithm
    {
        public override string ToString()
        {
            return "Manual Player Algo";
        }

        private bool IsValidIndex(int index, int maxCards)
        {
            return index >= -1 && index < maxCards;
        }

        public Color ChooseColor(IPlayersHandler playersHandler, IUserCommunicator userCommunicator)
        {
            return userCommunicator.GetColorFromUserEnum<CardColorsEnum>();
        }

        public Card? ChooseCard(Func<Card, bool> isSimilarTo, 
            Player player,
            IPlayersHandler playersHandler,
            IServiceProvider serviceProvider)
        {
            ICardsHandler cardsHandler = serviceProvider.GetRequiredService<ICardsHandler>();
            IUserCommunicator userCommunicator = serviceProvider.GetRequiredService<IUserCommunicator>();
            Card topDiscard = cardsHandler.GetTopDiscard();
            Player currentPlayer = playersHandler.GetCurrentPlayer();

            userCommunicator.SendAlertMessage($"The top deck card is {topDiscard}");
            currentPlayer.PlayerCards = OrderPlayerCardByColor(currentPlayer);
            userCommunicator.SendMessageToUser(currentPlayer.ToString());
            userCommunicator.SendAlertMessage($"Please choose one of your cards by index, " +
                $"-1 to draw a card");

            return ChooseValidCard(userCommunicator, currentPlayer, isSimilarTo);
        }

        private Card? ChooseValidCard(IUserCommunicator userCommunicator, 
            Player currentPlayer, Func<Card, bool> isSimilarTo)
        {
            if (!int.TryParse(userCommunicator.GetMessageFromUser(), out int index)
                || !IsValidIndex(index, currentPlayer.PlayerCards.Count))
            {
                userCommunicator.SendMessageToUser("please choose again the index of the card");
                return ChooseValidCard(userCommunicator, currentPlayer, isSimilarTo);
            }

            if (index == -1)
                return null;

            Card playerCard = currentPlayer.PlayerCards.ElementAt(index);
            if (!isSimilarTo(playerCard))
            {
                userCommunicator.SendMessageToUser("card does not meet the stacking rules");
                return ChooseValidCard(userCommunicator, currentPlayer, isSimilarTo);
            }

            return playerCard;
        }

        private List<Card> OrderPlayerCardByColor(Player currentPlayer)
        {
            return currentPlayer.PlayerCards.OrderBy(card =>
            {
                if (card is ColorCard colorCard)
                    return colorCard.GetColor().ToString();
                return Color.Empty.ToString();
            }).ToList();
        }
    }
}
