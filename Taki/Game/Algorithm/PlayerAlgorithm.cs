using System.Diagnostics;
using System.Drawing;
using Taki.Game.Cards;
using Taki.Game.Handlers;
using Taki.Game.Interfaces;
using Taki.Game.Players;

namespace Taki.Game.Algorithm
{
    internal class PlayerAlgorithm : IPlayerAlgorithm
    {
        public virtual Card? ChooseCard(Func<Card, bool> isSimilarTo, 
            Player player, GameHandlers gameHandlers)
        {
            if(player.PlayerCards.Count == 0) 
                return null;

            return player.PlayerCards.FirstOrDefault(card => isSimilarTo(card!), null);
        }

        public Color ChooseColor(GameHandlers gameHandlers)
        {
            Player currentPlayer = gameHandlers.GetPlayersHandler().CurrentPlayer;
            var colors = currentPlayer.PlayerCards
                .Where(card => card is ColorCard)
                .Select(card => ((ColorCard)card).GetColor())
                .GroupBy(c => c).ToList();

            try
            {
                return colors.OrderByDescending(color => color.Count())
                .ToList().First().FirstOrDefault(Color.Blue);
            }
            catch (Exception)
            {
                Debug.WriteLine("error with colors??");
                return Color.Blue;
            }
        }

        public override string ToString()
        {
            return "Player Algo";
        }
    }
}
