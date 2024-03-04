using MongoDB.Bson;
using Taki.Models.Algorithm;
using TakiApp.Dal;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Repositories
{
    internal class PlayersRepository : IPlayersRepository
    {
        private readonly IPlayersDal _playersDal;
        private readonly IDrawPileRepository _drawPileRepository;

        public PlayersRepository(
            IPlayersDal playersDal,
            IDrawPileRepository drawPileRepository)
        {
            _playersDal = playersDal;
            _drawPileRepository = drawPileRepository;
        }

        public async Task CreateManyAsync(List<Player> players)
        {
            await _playersDal.CreateManyAsync(players);
        }

        public async Task CreateNewAsync(Player player)
        {
            await _playersDal.CreateOneAsync(player);
        }

        public async Task<List<Player>> GetAllAsync()
        {
            var players = await _playersDal.FindAsync();

            return players;
        }

        public async Task<Player> PlayerDrawCardAsync(Player player)
        {
            var card = await _drawPileRepository.DrawCardAsync();

            if (card == null)
                return player;

            await _playersDal.DeleteAsync(player);

            player.Cards.Add(card);
            await _playersDal.CreateOneAsync(player);

            return player;
        }
    }
}
