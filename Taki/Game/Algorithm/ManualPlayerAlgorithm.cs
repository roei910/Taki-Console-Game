using System.Drawing;
using Taki.Game.Cards;
using Taki.Game.Handlers;
using Taki.Game.Interfaces;
using Taki.Game.Players;

namespace Taki.Game.Algorithm
{
    internal class ManualPlayerAlgorithm : IPlayerAlgorithm
    {
        public override string ToString()
        {
            return "Manual Player Algo";
        }

        private static bool IsValidIndex(int index, int maxCards)
        {
            return index >= -1 && index < maxCards;
        }

        public Color ChooseColor(GameHandlers gameHandlers)
        {
            return gameHandlers.GetMessageHandler().GetColorFromUserEnum<CardColorsEnum>();
        }

        public Card? ChooseCard(Func<Card, bool> isSimilarTo, 
            Player player, 
            GameHandlers gameHandlers)
        {
            IMessageHandler messageHandler = gameHandlers.GetMessageHandler();
            Card topDiscard = gameHandlers.GetCardsHandler().GetTopDiscard();
            Player currentPlayer = gameHandlers.GetPlayersHandler().CurrentPlayer;

            messageHandler.SendAlertMessage($"The top deck card is {topDiscard}");
            currentPlayer.PlayerCards = OrderPlayerCardByColor(currentPlayer);
            messageHandler.SendMessageToUser(currentPlayer.ToString());
            messageHandler.SendAlertMessage($"Please choose on of your cards by index, " +
                $"-1 to draw a card");

            return ChooseValidCard(messageHandler, currentPlayer, isSimilarTo);
        }

        private static Card? ChooseValidCard(IMessageHandler messageHandler, 
            Player currentPlayer, Func<Card, bool> isSimilarTo)
        {
            if (!int.TryParse(messageHandler.GetMessageFromUser(), out int index)
                || !IsValidIndex(index, currentPlayer.PlayerCards.Count))
            {
                messageHandler.SendMessageToUser("please choose again the index of the card");
                return ChooseValidCard(messageHandler, currentPlayer, isSimilarTo);
            }

            if (index == -1)
                return null;

            Card playerCard = currentPlayer.PlayerCards.ElementAt(index);
            if (!isSimilarTo(playerCard))
            {
                messageHandler.SendMessageToUser("card does not meet the stacking rules");
                return ChooseValidCard(messageHandler, currentPlayer, isSimilarTo);
            }

            return playerCard;
        }

        private static List<Card> OrderPlayerCardByColor(Player currentPlayer)
        {
            return currentPlayer.PlayerCards
                .GroupBy(card =>
                {
                    if (card is ColorCard colorCard)
                        return colorCard.GetColor();
                    return Color.Empty;
                })
                .ToList().SelectMany(x => x).ToList();
        }
    }
}
