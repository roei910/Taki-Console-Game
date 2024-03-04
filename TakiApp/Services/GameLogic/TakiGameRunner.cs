using MongoDB.Bson;
using Taki.Models.Algorithm;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Services.GameLogic
{
    public class TakiGameRunner : ITakiGameRunner
    {
        private readonly IUserCommunicator _userCommunicator;

        private readonly IPlayersRepository _playersRepository;
        private readonly IGameSettingsRepository _gameSettingsRepository;
        private readonly IDiscardPileRepository _discardPileRepository;
        private readonly IDrawPileRepository _drawPileRepository;

        private readonly IGameInitializer _gameInitializer;
        private readonly IGameTurnService _gameTurnService;

        public TakiGameRunner(IGameInitializer gameInitializer, 
            IGameTurnService gameTurnService, IUserCommunicator userCommunicator,
            IPlayersRepository playersRepository, IDiscardPileRepository discardPileRepository,
            IDrawPileRepository drawPileRepository, IGameSettingsRepository gameSettingsRepository)
        {
            _gameInitializer = gameInitializer;
            _gameTurnService = gameTurnService;
            _userCommunicator = userCommunicator;
            _playersRepository = playersRepository;
            _gameSettingsRepository = gameSettingsRepository;
            _discardPileRepository = discardPileRepository;
            _drawPileRepository = drawPileRepository;
        }

        public async Task Run()
        {
            await _gameInitializer.InitializeGame();
            var gameSettings = _gameInitializer.GetGameSettings();

            if (gameSettings!.IsOnline)
            {
                await StartOnlineGame(gameSettings);
                return;
            }

            await StartNormal();
        }

        private async Task StartOnlineGame(GameSettings gameSettings)
        {
            if(gameSettings.HasGameStarted)
            {
                _userCommunicator.SendErrorMessage("the game already started without you, GoodBye!");
                return;
            }

            Player player = new()
            {
                Id = ObjectId.GenerateNewId(),
                Name = _userCommunicator.GetMessageFromUser("Please enter a name for your player"),
                LastCheckIn = DateTime.UtcNow,
                PlayerAlgorithm = typeof(ManualPlayerAlgorithm).ToString(),
                Cards = [],
                IsPlaying = false
            };

            await _playersRepository.CreateNewAsync(player);
            var players = await _playersRepository.GetAllAsync();

            await _gameSettingsRepository.WaitGameStart(players.Count);

            while (true)
            {
                _userCommunicator.SendAlertMessage("waiting for your turn!");

                await _gameTurnService.WaitTurnById(player.Id);
                _gameTurnService.PlayTurnById(player.Id);
            }
        }

        private async Task StartNormal()
        {
            var players = await _playersRepository.GetAllAsync();

            await _gameSettingsRepository.WaitGameStart(players.Count);
        }
    }
}
