using MongoDB.Bson;
using Taki.Models.Algorithm;
using TakiApp.Factories;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Services.GameLogic
{
    public class GameInitializer : IGameInitializer
    {
        private readonly IUserCommunicator _userCommunicator;
        private readonly IGameSettingsRepository _gameSettingsRepository;
        private readonly IPlayersRepository _playersRepository;
        private readonly CardsFactory _cardsFactory;
        private readonly IDrawPileRepository _drawPileRepository;
        private readonly IDiscardPileRepository _discardPileRepository;
        private GameSettings? _gameSettings;

        public GameInitializer(IUserCommunicator userCommunicator, IGameSettingsRepository gameSettingsRepository,
            IPlayersRepository playersRepository, CardsFactory cardsFactory, IDrawPileRepository drawPileRepository, 
            IDiscardPileRepository discardPileRepository)
        {
            _userCommunicator = userCommunicator;
            _gameSettingsRepository = gameSettingsRepository;
            _playersRepository = playersRepository;
            _cardsFactory = cardsFactory;
            _drawPileRepository = drawPileRepository;
            _discardPileRepository = discardPileRepository;
        }

        public GameSettings GetGameSettings()
        {
            return _gameSettings!;
        }

        public async Task InitializeGame()
        {
            _gameSettings = _gameSettingsRepository.GetGameSettingsAsync().Result;

            if(_gameSettings == null)
            {
                _userCommunicator.SendMessageToUser("Building a new Taki Game!");
                var isOnline = _userCommunicator.GetMessageFromUser("Please enter type of game: online or normal");
                var typeOfGame = _userCommunicator.GetMessageFromUser("Please enter pyramid or normal");
                var numberOfPlayerCards = _userCommunicator.GetNumberFromUser("Please enter number of player cards");
                var numberOfPlayers = _userCommunicator.GetNumberFromUser("Please enter number of players");

                _gameSettings = new GameSettings()
                {
                    Id = ObjectId.GenerateNewId(),
                    HasGameStarted = false,
                    IsOnline = isOnline == "online",
                    NumberOfPlayerCards = numberOfPlayerCards,
                    TypeOfGame = typeOfGame,
                    NumberOfPlayers = numberOfPlayers,
                };

                await _gameSettingsRepository.CreateGameSettings(_gameSettings);

                var cards = _cardsFactory.GenerateDeck();

                await _drawPileRepository.AddManyRandomAsync(cards);
                await InitializeCards();
            }

            //TODO: game already setting up in the background we need to check if we can enter or not.
            if (!_gameSettings.IsOnline)
            {
                await InitializeNormal();

                return;
            }

        }

        private async Task InitializeNormal()
        {
            var players =  Enumerable.Range(0, _gameSettings!.NumberOfPlayers)
                .Select(x =>
                {
                    Player player = new Player()
                    {
                        Id = ObjectId.GenerateNewId(),
                        Name = _userCommunicator.GetMessageFromUser("Please enter a name for your player"),
                        LastCheckIn = DateTime.UtcNow,
                        PlayerAlgorithm = typeof(ManualPlayerAlgorithm).ToString(),
                        Cards = [],
                        IsPlaying = false
                    };

                    return player;
                }).ToList();

            await _playersRepository.CreateManyAsync(players);
            await InitializeCards();

            players.ForEach(async p =>
            {
                while (p.Cards.Count < _gameSettings.NumberOfPlayerCards)
                    await _playersRepository.PlayerDrawCardAsync(p);
            });
        }

        private async Task InitializeCards()
        {
            await _drawPileRepository.DeleteAllAsync();
            await _discardPileRepository.DeleteAllAsync();

            await _drawPileRepository.AddManyRandomAsync(_cardsFactory.GenerateDeck());
            var drawCard = await _drawPileRepository.DrawCardAsync();

            await _discardPileRepository.AddCard(drawCard);
        }
    }
}
