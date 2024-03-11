using System.Drawing;
using TakiApp.Shared.Interfaces;
using TakiApp.Shared.Models;

namespace TakiApp.Services.Algorithms
{
    public class AlgorithmService : IAlgorithmService
    {
        private readonly List<IPlayerAlgorithm> _playerAlgorithms;

        public AlgorithmService(List<IPlayerAlgorithm> playerAlgorithms)
        {
            _playerAlgorithms = playerAlgorithms;
        }

        public Card? PickCard(Player currentPlayer, Func<Card, bool> canStackOnTopDiscard, string? elseMessage = null)
        {
            var algorithm = MatchAlgorithm(currentPlayer);

            var chosenCard = algorithm.ChooseCard(canStackOnTopDiscard, currentPlayer.Cards, elseMessage);

            return chosenCard;
        }

        public Player PickOtherPlayer(Player currentPlayer, List<Player> players)
        {
            var algorithm = MatchAlgorithm(currentPlayer);

            return algorithm.ChoosePlayer(players);
        }

        public Color ChooseColor(Player player)
        {
            var algo = MatchAlgorithm(player);

            var color = algo.ChooseColor(player.Cards);

            return color;
        }

        private IPlayerAlgorithm MatchAlgorithm(Player player)
        {
            var found = _playerAlgorithms.Where(algo => algo.ToString() == player.PlayerAlgorithm).FirstOrDefault();

            return found ?? throw new Exception("Couldnt find algorithm in the list");
        }
    }
}
