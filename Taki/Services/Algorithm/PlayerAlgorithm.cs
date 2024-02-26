using System.Drawing;
using Taki.Models.Cards;
using Taki.Models.Players;
using Taki.Shared.Interfaces;

namespace Taki.Models.Algorithm
{
    internal class PlayerAlgorithm : IPlayerAlgorithm
    {
        public virtual Card? ChooseCard(Func<Card, bool> isSimilarTo, List<Card> playerCards, string? elseMessage = null)
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
                .Where(card => card is ColorCard)
                .Select(card => ((ColorCard)card).GetColor())
                .GroupBy(c => c);

            if (colors.Count() == 0)
                return Color.Blue;

            return colors.OrderByDescending(color => color.Count()).First().FirstOrDefault(Color.Blue);
        }

        public Player ChoosePlayer(Player currentPlayer, IPlayersHolder playersHolder)
        {
            var players = playersHolder.Players
                .Where(player => !player.Equals(currentPlayer)).ToList();

            return players.OrderBy(val => Guid.NewGuid().ToString()).First();
        }

        public override string ToString()
        {
            return "Player Algo";
        }
    }
}
