using System.Drawing;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace Taki.Models.Algorithm
{
    public class PlayerAlgorithm : IPlayerAlgorithm
    {
        public Card? ChooseCard(Func<Card, bool> isSimilarTo, List<Card> playerCards, string? elseMessage = null)
        {
            if (playerCards.Count == 0)
                return null;

            Card? playerCard = Task.Run(async () =>
            {
                Card? card = playerCards.FirstOrDefault(card => isSimilarTo(card!));
                await Task.Delay(2000);

                return card;
            }).Result;

            return playerCard;
        }

        public Color ChooseColor(List<Card> playerCards)
        {
            var colors = playerCards
                .Select(card => Color.FromName(card.CardColor))
                .GroupBy(c => c);

            if (colors.Count() == 0)
                return Color.Blue;

            return colors.OrderByDescending(color => color.Count()).First().FirstOrDefault(Color.Blue);
        }

        public Player ChoosePlayer(List<Player> players)
        {
            return players.OrderBy(val => Guid.NewGuid().ToString()).First();
        }
    }
}
