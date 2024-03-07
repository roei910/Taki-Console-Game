using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Repositories
{
    public class GameSettingsRepository : IGameSettingsRepository
    {
        private readonly IDal<GameSettings> _gameSettingsDal;
        private readonly IDal<Player> _playersDal;
        private readonly IDrawPileDal _drawPileDal;
        private readonly IDiscardPileDal _discardPileDal;
        private readonly IUserCommunicator _userCommunicator;

        public GameSettingsRepository(IDal<GameSettings> dal,
            IUserCommunicator userCommunicator, IDal<Player> playersDal,
            IDrawPileDal drawPileDal, IDiscardPileDal discardPileDal)
        {
            _gameSettingsDal = dal;
            _userCommunicator = userCommunicator;
            _playersDal = playersDal;
            _drawPileDal = drawPileDal;
            _discardPileDal = discardPileDal;
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

                var players = await _playersDal.FindAsync();
                var first = players[0];
                first.IsPlaying = true;

                await _playersDal.UpdateOneAsync(first);

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
            await _playersDal.DeleteAllAsync();
            await _discardPileDal.DeleteAllAsync();
            await _drawPileDal.DeleteAllAsync();
            await _gameSettingsDal.DeleteAllAsync();
        }

        public async Task FinishGameAsync()
        {
            var gameSettings = await GetGameSettingsAsync();

            gameSettings!.HasGameEnded = true;

            await _gameSettingsDal.UpdateOneAsync(gameSettings);

            var players = await _playersDal.FindAsync();
            players = players.Select(x =>
            {
                x.IsPlaying = true;

                return x;
            }).ToList();

            await _playersDal.UpdateManyAsync(players);
        }

        public async Task UpdateWinnersAsync(string name)
        {
            var gameSettings = await GetGameSettingsAsync();

            gameSettings!.winners.Add(name);

            await _gameSettingsDal.UpdateOneAsync(gameSettings);
        }
    }
}
