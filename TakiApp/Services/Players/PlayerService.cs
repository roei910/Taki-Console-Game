using System.Drawing;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Services.Players
{
    public class PlayerService : IPlayerService
    {
        private readonly List<IPlayerAlgorithm> _playerAlgorithms;
        private readonly IPlayersRepository _playersRepository;
        private readonly IDrawPileRepository _drawPileRepository;

        public PlayerService(List<IPlayerAlgorithm> playerAlgorithms,
            IPlayersRepository playersRepository, IDrawPileRepository drawPileRepository)
        {
            _playerAlgorithms = playerAlgorithms;
            this._playersRepository = playersRepository;
            this._drawPileRepository = drawPileRepository;
        }

        public Card? PickCard(Player currentPlayer, Func<Card, bool> canStackOnTopDiscard)
        {
            var algorithm = MatchAlgorithm(currentPlayer);

            var chosenCard = algorithm.ChooseCard(canStackOnTopDiscard, currentPlayer.Cards);

            return chosenCard;
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

        public async Task DrawCard(Player currentPlayer)
        {
            Card? drawCard = await _drawPileRepository.DrawCardAsync();

            if (drawCard is null)
                return;

            currentPlayer.Cards.Add(drawCard);
            await _playersRepository.UpdatePlayer(currentPlayer);
            await _playersRepository.NextPlayerAsync(currentPlayer);
        }
    }
}
