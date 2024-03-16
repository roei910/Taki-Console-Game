using Taki.Models.Algorithm;
using TakiApp.Shared.Interfaces;
using TakiApp.Shared.Models;

namespace TakiApp.Repositories
{
    public class GameSettingsRepository : IGameSettingsRepository
    {
        private readonly IDal<GameSettings> _gameSettingsDal;
        private readonly IDrawPileDal _drawPileDal;
        private readonly IDiscardPileDal _discardPileDal;
        private readonly IPlayersRepository _playersRepository;
        private readonly IGameScore _gameScore;
        private readonly IUserCommunicator _userCommunicator;

        public GameSettingsRepository(IDal<GameSettings> dal,
            IUserCommunicator userCommunicator, IDrawPileDal drawPileDal, 
            IDiscardPileDal discardPileDal, IPlayersRepository playersRepository,
            IGameScore gameScore)
        {
            _gameSettingsDal = dal;
            _userCommunicator = userCommunicator;
            _drawPileDal = drawPileDal;
            _discardPileDal = discardPileDal;
            _playersRepository = playersRepository;
            _gameScore = gameScore;
        }

        public async Task CreateGameSettings(GameSettings gameSettings)
        {
            await _gameSettingsDal.DeleteAllAsync();
            await _gameSettingsDal.CreateOneAsync(gameSettings);
        }

        public async Task WaitGameStartAsync(int numberOfCurrentPlayers)
        {
            var gameSettings = await GetGameSettingsAsync();

            //TODO: remove messages from repository
            var onlineSubMessage = gameSettings!.IsOnline ? "online" : "localy";
            var message = $"Starting a {gameSettings!.TypeOfGame} game {onlineSubMessage}!";

            if (gameSettings!.NumberOfPlayers == numberOfCurrentPlayers)
            {
                gameSettings.HasGameStarted = true;
                await _gameSettingsDal.UpdateOneAsync(gameSettings);

                var players = await _playersRepository.GetAllAsync();
                var first = players[0];
                first.IsPlaying = true;

                await _playersRepository.UpdatePlayerAsync(first);

                _userCommunicator.SendAlertMessage(message);

                return;
            }

            _userCommunicator.SendAlertMessage("Waiting for game to start!\n");

            while (gameSettings!.HasGameStarted == false)
            {
                await Task.Delay(3000);
                gameSettings = await GetGameSettingsAsync();
            }
        }

        public async Task<GameSettings?> GetGameSettingsAsync()
        {
            var gameSettings = await _gameSettingsDal.FindAsync();

            return gameSettings.FirstOrDefault();
        }

        public async Task DeleteGameAsync()
        {
            await _playersRepository.DeleteAllAsync();
            await _discardPileDal.DeleteAllAsync();
            await _drawPileDal.DeleteAllAsync();
            await _gameSettingsDal.DeleteAllAsync();
        }

        public async Task<GameSettings> FinishGameAsync()
        {
            var gameSettings = await GetGameSettingsAsync();

            gameSettings!.HasGameEnded = true;

            await _gameSettingsDal.UpdateOneAsync(gameSettings);

            var players = await _playersRepository.GetAllAsync();
            players = players.Select(x =>
            {
                x.IsPlaying = true;

                return x;
            }).ToList();

            await _playersRepository.UpdateManyAsync(players);

            return gameSettings;
        }

        public async Task<GameSettings> UpdateWinnersAsync(Player player)
        {
            var gameSettings = await GetGameSettingsAsync();

            if (gameSettings!.winners.Count == 0 && player.PlayerAlgorithm == typeof(ManualPlayerAlgorithm).ToString())
                _gameScore.SetScoreByName(player.Name! ,player.Score + 1);
            
            gameSettings!.winners.Add(player.Name!);

            await _gameSettingsDal.UpdateOneAsync(gameSettings);

            if (gameSettings!.winners.Count == gameSettings.NumberOfWinners)
            {
                var winnersList = gameSettings.winners.Select((winner, index) => $"{index + 1}. {winner}").ToList();
                var message = "game finished the winners are:\n" + string.Join("\n", winnersList) + "\n";

                await _playersRepository.SendMessagesToPlayersAsync("System", message);
                gameSettings = await FinishGameAsync();
            }

            return gameSettings;
        }
    }
}