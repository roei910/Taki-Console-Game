﻿using MongoDB.Bson;
using MongoDB.Driver.Linq;
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

        public async Task AddMessagesToPlayersAsync(Player playerSent, string message)
        {
            var players = await _playersDal.FindAsync();
            players.Remove(playerSent);
            players.ForEach(p => p.Messages.Add($"Message from {playerSent.Name}: {message}"));

            await _playersDal.UpdateManyAsync(players);
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

        public async Task DrawCardsAsync(Player player, int cardsToDraw)
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

        public async Task<Player> GetPlayerByIdAsync(ObjectId playerId)
        {
            var player = await _playersDal.FindOneAsync(playerId);

            return player;
        }

        public async Task NextPlayerAsync()
        {
            var players = await _playersDal.FindAsync();
            
            var currentPlayer = players.Where(x => x.IsPlaying).First();
            var nextPlayer = GetNextN(players, currentPlayer).ElementAt(0);

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

        public async Task SkipPlayers(int playersToSkip = 1)
        {
            var players = await _playersDal.FindAsync();

            var currentPlayer = players.Where(x => x.IsPlaying).First();
            var nextNPlayers = GetNextN(players, currentPlayer, playersToSkip + 1);

            currentPlayer.IsPlaying = false;
            await _playersDal.UpdateOneAsync(currentPlayer);

            for (int i = 0; i < nextNPlayers.Count - 1; i++)
                nextNPlayers[i].Messages.Add($"You were stopped by {currentPlayer.Name}");

            nextNPlayers.Last().IsPlaying = true;
            await _playersDal.UpdateManyAsync(nextNPlayers);
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

        private List<Player> GetNextN(List<Player> players, Player currentPlayer, int numberOfPlayers = 1)
        {
            var orderedPlayers = players.OrderBy(x => x.Order).ToList();
            var currentPlayerIndex = orderedPlayers.IndexOf(currentPlayer);
            var playersBefore = orderedPlayers.GetRange(0, currentPlayerIndex);

            orderedPlayers.RemoveRange(0, currentPlayerIndex);
            orderedPlayers.AddRange(playersBefore);
            orderedPlayers.RemoveAt(0);
            orderedPlayers.Add(currentPlayer);

            return orderedPlayers.Take(numberOfPlayers).ToList();
        }
    }
}
