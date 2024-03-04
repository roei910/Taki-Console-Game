using MongoDB.Bson;
using MongoDB.Driver;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Repositories
{
    public class PlayersRepository : IPlayersRepository
    {
        private readonly IDal<Player> _playersDal;
        private readonly IDrawPileRepository _drawPileRepository;

        public PlayersRepository(
            IDal<Player> playersDal,
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

        public async Task<Player> GetCurrentPlayerAsync()
        {
            var players = await _playersDal.FindAsync();

            return players[0];
        }

        public async Task NextPlayerAsync(Player player)
        {
            await _playersDal.DeleteAsync(player);

            player.IsPlaying = false;

            await _playersDal.CreateOneAsync(player);

            var current = await GetCurrentPlayerAsync();

            current.IsPlaying = true;

            await _playersDal.UpdateOneAsync(current);
        }

        public async Task<Player> PlayerDrawCardAsync(Player player)
        {
            var card = await _drawPileRepository.DrawCardAsync();

            if (card == null)
                return player;

            player.Cards.Add(card);
            await _playersDal.UpdateOneAsync(player);

            return player;
        }

        public async Task WaitTurnAsync(ObjectId playerId)
        {
            while (true)
            {
                var player = await _playersDal.FindOneAsync(playerId);

                if (player != null && player.IsPlaying)
                    return;

                await Task.Run(async () => await Task.Delay(3000));
            }
        }
    }
}
