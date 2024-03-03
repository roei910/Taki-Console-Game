using MongoDB.Bson;
using Taki.Models.Algorithm;
using TakiApp.Dal;
using TakiApp.Factories;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Services.GameLogic
{
    internal class TakiGameRunner : ITakiGameRunner
    {
        private readonly IGameInitializer _gameInitializer;
        private readonly IGameTurnService _gameTurnService;
        private readonly IUserCommunicator _userCommunicator;
        private readonly IPlayersDal _playersDal;
        private readonly CardsFactory _cardsFactory;
        private readonly IDal<Card> _discardPile;
        private readonly IDal<Card> _drawPile;

        public TakiGameRunner(IGameInitializer gameInitializer, 
            IGameTurnService gameTurnService, IUserCommunicator userCommunicator,
            IPlayersDal playersDal, CardsFactory cardsFactory, List<IDal<Card>> cardDals)
        {
            _gameInitializer = gameInitializer;
            _gameTurnService = gameTurnService;
            _userCommunicator = userCommunicator;
            _playersDal = playersDal;
            _cardsFactory = cardsFactory;
            _drawPile = cardDals.Where(dal => dal.GetType() == typeof(DrawPileDal)).First();
            _discardPile = cardDals.Where(dal => dal.GetType() == typeof(DiscardPileDal)).First();
        }

        public async Task Run()
        {
            _gameInitializer.InitializeGame();
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

            //TODO: create a new player get his id and start playing in that user.
            Player player = new Player()
            {
                Id = ObjectId.GenerateNewId(),
                Name = _userCommunicator.GetMessageFromUser("Please enter a name for your player"),
                LastCheckIn = DateTime.UtcNow,
                PlayerAlgorithm = typeof(ManualPlayerAlgorithm).ToString(),
                Cards = [],
                IsPlaying = false
            };

            await _playersDal.CreateOneAsync(player);

            while (true)
            {
                await _gameTurnService.WaitTurnById(player.Id);
                _gameTurnService.PlayTurnById(player.Id);
            }
        }

        private async Task StartNormal()
        {
            //TODO: normal game from local db only
            await _drawPile.DeleteAllAsync();
            await _discardPile.DeleteAllAsync();
            await _drawPile.CreateManyAsync(_cardsFactory.GenerateDeck());
        }
    }
}
