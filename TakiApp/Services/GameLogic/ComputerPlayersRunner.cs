using MongoDB.Bson;
using Taki.Models.Algorithm;
using TakiApp.Shared.Interfaces;
using TakiApp.Shared.Models;

namespace TakiApp.Services.GameLogic
{
    public class ComputerPlayersRunner : IComputerPlayersRunner
    {
        private readonly IPlayersRepository _playersRepository;
        private readonly IGameTurnService _gameTurnService;
        private readonly IGameSettingsRepository _gameSettingsRepository;

        public ComputerPlayersRunner(IPlayersRepository playersRepository, IGameTurnService gameTurnService,
            IGameSettingsRepository gameSettingsRepository)
        {
            _playersRepository = playersRepository;
            _gameTurnService = gameTurnService;
            _gameSettingsRepository = gameSettingsRepository;
        }

        public async Task<List<Player>> GenerateComputerPlayers()
        {
            var gameSettings = await _gameSettingsRepository.GetGameSettingsAsync();

            var numberOfComputerPlayers = gameSettings!.NumberOfPlayers - gameSettings!.NumberOfManualPlayers;

            List<Player> result = new List<Player>();

            for (int i = 0; i < numberOfComputerPlayers; i++)
            {
                var player = await GeneragePlayer(i);

                result.Add(player);
            }

            await RunPlayers();

            return result;
        }

        public async Task<Player> GeneragePlayer(int index)
        {
            Player onlinePlayer = new()
            {
                Id = ObjectId.GenerateNewId(),
                Name = $"Player {index + 1}",
                LastCheckIn = DateTime.UtcNow,
                PlayerAlgorithm = typeof(PlayerAlgorithm).ToString(),
                Cards = [],
                IsPlaying = false
            };

            await _playersRepository.CreatePlayerAsync(onlinePlayer);

            return onlinePlayer;
        }

        public async Task RunPlayers()
        {
            var players = await _playersRepository.GetAllAsync();
            var computerPlayers = players.Where(x => x.PlayerAlgorithm == typeof(PlayerAlgorithm).ToString())
                .ToList();

            computerPlayers.ForEach(async p => await RunPlayer(p));
        }

        private async Task RunPlayer(Player player)
        {
            await _gameTurnService.WaitTurnByIdAsync(player.Id, false);

            var gameSettings = await _gameSettingsRepository.GetGameSettingsAsync();

            if (gameSettings!.HasGameEnded)
                return;

            player = await _gameTurnService.PlayTurnByIdAsync(player.Id, false);

            if (player.Cards.Count == 0)
            {
                await _gameSettingsRepository.UpdateWinnersAsync(player);

                await _gameTurnService.WaitGameEndAsync(player.Id, false);

                return;
            }

            await RunPlayer(player);
        }
    }
}