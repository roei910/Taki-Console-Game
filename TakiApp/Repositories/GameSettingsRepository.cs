using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Repositories
{
    public class GameSettingsRepository : IGameSettingsRepository
    {
        private readonly IDal<GameSettings> _dal;
        private readonly IUserCommunicator _userCommunicator;

        public GameSettingsRepository(IDal<GameSettings> dal,
            IUserCommunicator userCommunicator)
        {
            _dal = dal;
            _userCommunicator = userCommunicator;
        }

        public async Task CreateGameSettings(GameSettings gameSettings)
        {
            await _dal.DeleteAllAsync();
            await _dal.CreateOneAsync(gameSettings);
        }

        public async Task WaitGameStart(int numberOfCurrentPlayers)
        {
            var gameSettings = await GetGameSettingsAsync();

            var onlineSubMessage = gameSettings!.IsOnline ? "online" : "localy";
            var message = $"Starting a {gameSettings!.TypeOfGame} game {onlineSubMessage}!";

            if (gameSettings?.NumberOfPlayers == numberOfCurrentPlayers)
            {
                gameSettings.HasGameStarted = true;
                await _dal.UpdateOneAsync(gameSettings);

                _userCommunicator.SendAlertMessage(message);

                return;
            }

            _userCommunicator.SendAlertMessage("Waiting for game to start!");


            while (gameSettings?.HasGameStarted == false)
            {
                await Task.Delay(3000);
                gameSettings = await GetGameSettingsAsync();
            }
        }

        public async Task<GameSettings?> GetGameSettingsAsync()
        {
            var gameSettings = await _dal.FindAsync();

            return gameSettings.FirstOrDefault();
        }
    }
}
