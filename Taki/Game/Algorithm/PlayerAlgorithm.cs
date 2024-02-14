using System.Drawing;
using Taki.Game.Cards;
using Taki.Game.Handlers;
using Taki.Game.Messages;
using Taki.Game.Players;

namespace Taki.Game.Algorithm
{
    internal class PlayerAlgorithm : IPlayerAlgorithm
    {
        public virtual Card? ChooseCard(Func<Card, bool> isSimilarTo, 
            Player player, IPlayersHandler playersHandler, IServiceProvider serviceProvider)
        {
            if(player.PlayerCards.Count == 0) 
                return null;

            return player.PlayerCards.FirstOrDefault(card => isSimilarTo(card!));
        }

        public Color ChooseColor(IPlayersHandler playersHandler, IUserCommunicator userCommunicator)
        {
            Player currentPlayer = playersHandler.GetCurrentPlayer();
            var colors = currentPlayer.PlayerCards
                .Where(card => card is ColorCard)
                .Select(card => ((ColorCard)card).GetColor())
                .GroupBy(c => c);

            if (colors.Count() == 0)
                return Color.Blue;

            return colors.OrderByDescending(color => color.Count()).First().FirstOrDefault(Color.Blue);
        }

        public override string ToString()
        {
            return "Player Algo";
        }
    }
}
