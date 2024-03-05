using TakiApp.Interfaces;

namespace TakiApp.Services.GameLogic
{
    public class TakiGameRunner : ITakiGameRunner
    {
        private readonly IUserCommunicator _userCommunicator;

        private readonly IPlayersRepository _playersRepository;
        private readonly IGameSettingsRepository _gameSettingsRepository;
        private readonly IGameInitializer _gameInitializer;
        private readonly IGameTurnService _gameTurnService;

        public TakiGameRunner(IGameInitializer gameInitializer, 
            IGameTurnService gameTurnService, IUserCommunicator userCommunicator,
            IPlayersRepository playersRepository, IGameSettingsRepository gameSettingsRepository)
        {
            _gameInitializer = gameInitializer;
            _gameTurnService = gameTurnService;
            _userCommunicator = userCommunicator;
            _playersRepository = playersRepository;
            _gameSettingsRepository = gameSettingsRepository;
        }

        public async Task Run()
        {
            await _gameInitializer.InitializeGame();
            var gameSettings = _gameInitializer.GetGameSettings();

            if (gameSettings!.IsOnline)
            {
                await StartOnlineGame();
                return;
            }

            await StartNormal();
        }

        private async Task StartOnlineGame()
        {
            var player = _gameInitializer.GetPlayer;

            while (true)
            {
                _userCommunicator.SendAlertMessage("waiting for your turn!");

                await _gameTurnService.WaitTurnByIdAsync(player.Id);
                await _gameTurnService.PlayTurnByIdAsync(player.Id);
                await _playersRepository.NextPlayerAsync();
            }
        }

        private async Task StartNormal()
        {
            var players = await _playersRepository.GetAllAsync();

            await _gameSettingsRepository.WaitGameStartAsync(players.Count);
        }
    }
}
