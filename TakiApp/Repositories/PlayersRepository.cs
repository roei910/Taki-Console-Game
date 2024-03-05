using MongoDB.Bson;
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
            int count = 0;
            foreach(Player player in players)
                player.Order = count++;

            await _playersDal.CreateManyAsync(players);
        }

        public async Task CreatePlayerAsync(Player player)
        {
            var players = await _playersDal.FindAsync();

            player.Order = players.Count;

            await _playersDal.CreateOneAsync(player);
        }

        public async Task DeleteAllAsync()
        {
            await _playersDal.DeleteAllAsync();
        }

        public async Task DrawCards(Player player, int cardsToDraw)
        {
            List<Card> cards = await _drawPileRepository.DrawCardsAsync(cardsToDraw);

            player.Cards.AddRange(cards);
            await _playersDal.UpdateOneAsync(player);
        }

        public async Task<List<Player>> GetAllAsync()
        {
            var players = await _playersDal.FindAsync();

            return players;
        }

        public async Task<Player> GetCurrentPlayerAsync()
        {
            var players = await _playersDal.FindAsync();

            var ordered = players.Where(x => x.IsPlaying).ToList();

            return ordered.First();
        }

        public async Task NextPlayerAsync()
        {
            var players = await _playersDal.FindAsync();
            var orderedPlayers = players.OrderBy(x => x.Order).ToList();
            
            var currentPlayer = orderedPlayers.Where(x => x.IsPlaying).First();
            var nextPlayer = orderedPlayers.ElementAtOrDefault(orderedPlayers.IndexOf(currentPlayer) + 1) ?? orderedPlayers[0];

            currentPlayer.IsPlaying = false;
            await _playersDal.UpdateOneAsync(currentPlayer);

            nextPlayer.IsPlaying = true;
            await _playersDal.UpdateOneAsync(nextPlayer);
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

        public async Task UpdateOrder(List<Player> players)
        {
            int count = 0;
            foreach (Player player in players)
                player.Order = count++;

            await _playersDal.UpdateManyAsync(players);
        }

        public async Task UpdatePlayer(Player player)
        {
            await _playersDal.UpdateOneAsync(player);
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
