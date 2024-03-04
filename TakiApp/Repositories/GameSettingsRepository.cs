using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Repositories
{
    public class GameSettingsRepository : IGameSettingsRepository
    {
        private readonly IDal<GameSettings> _gameSettingsDal;
        private readonly IDal<Player> _playersDal;
        private readonly IUserCommunicator _userCommunicator;

        public GameSettingsRepository(IDal<GameSettings> dal,
            IUserCommunicator userCommunicator, IDal<Player> playersDal)
        {
            _gameSettingsDal = dal;
            _userCommunicator = userCommunicator;
            _playersDal = playersDal;
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

            _userCommunicator.SendAlertMessage("Waiting for game to start!");

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
    }
}
