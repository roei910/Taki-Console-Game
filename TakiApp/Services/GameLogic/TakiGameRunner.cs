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

            if (gameSettings!.HasGameStarted)
                return;

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
                _userCommunicator.SendAlertMessage("Waiting for your turn!\n");

                await _gameTurnService.WaitTurnByIdAsync(player.Id);

                var gameSettings = await _gameSettingsRepository.GetGameSettingsAsync();

                if (gameSettings!.HasGameEnded)
                {
                    _userCommunicator.SendMessageToUser("The game ended, hope you had fun");

                    return;
                }

                player = await _gameTurnService.PlayTurnByIdAsync(player.Id);

                if (player.Cards.Count == 0)
                {
                    _userCommunicator.SendMessageToUser("You finished your hand!");

                    await _gameSettingsRepository.UpdateWinnersAsync(player.Name!);

                    await _gameTurnService.WaitGameEndAsync(player.Id);

                    return;
                }
            }
        }

        private async Task StartNormal()
        {
            while (true)
            {
                var player = await _playersRepository.GetCurrentPlayerAsync();

                _userCommunicator.SendMessageToUser($"User playing: {player.Name}");

                var gameSettings = await _gameSettingsRepository.GetGameSettingsAsync();

                if (gameSettings!.HasGameEnded)
                {
                    _userCommunicator.SendMessageToUser("The game ended, hope you had fun");

                    //TODO: read messages
                    return;
                }

                player = await _gameTurnService.PlayTurnByIdAsync(player.Id);

                if (player.Cards.Count == 0)
                {
                    _userCommunicator.SendMessageToUser("You finished your hand!");

                    await _gameSettingsRepository.UpdateWinnersAsync(player.Name!);
                }
            }
        }
    }
}
