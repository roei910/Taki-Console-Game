using System.Drawing;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Services.Players
{
    public class PlayerService : IPlayerService
    {
        private readonly List<IPlayerAlgorithm> _playerAlgorithms;

        public PlayerService(List<IPlayerAlgorithm> playerAlgorithms)
        {
            _playerAlgorithms = playerAlgorithms;
        }

        public Card? PickCard(Player currentPlayer, Func<Card, bool> canStackOnTopDiscard)
        {
            var algorithm = MatchAlgorithm(currentPlayer);

            var chosenCard = algorithm.ChooseCard(canStackOnTopDiscard, currentPlayer.Cards);

            return chosenCard;
        }

        public void AddCard(Player currentPlayer, Card card)
        {

            throw new NotImplementedException();
        }

        public Player PickOtherPlayer(Player currentPlayer, List<Player> players)
        {
            var algorithm = MatchAlgorithm(currentPlayer);

            throw new NotImplementedException();
        }

        public Color ChooseColor(Player player)
        {
            var algo = MatchAlgorithm(player);

            return algo.ChooseColor(player.Cards);
        }

        private IPlayerAlgorithm MatchAlgorithm(Player player)
        {
            var found = _playerAlgorithms.Where(algo => algo.ToString() == player.PlayerAlgorithm).FirstOrDefault();

            return found ?? throw new Exception("Couldnt find algorithm in the list");
        }
    }
}
