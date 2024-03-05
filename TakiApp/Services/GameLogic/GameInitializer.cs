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
        private Player? _onlinePlayer;

        public Player GetPlayer => _onlinePlayer!;

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

                await InitializeCards();

                await GenerateOnlinePlayer();

                await _gameSettingsRepository.WaitGameStartAsync(1);

                await DrawCards();

                return;
            }

            if (_gameSettings!.HasGameStarted)
            {
                _userCommunicator.SendErrorMessage("the game already started without you, GoodBye!");

                //TODO: check if the game is playing or can be removed
                return;
            }

            if (!_gameSettings.IsOnline)
            {
                await InitializeNormal();

                return;
            }

            await InitializeOnline();
        }

        private async Task DrawCards()
        {
            var players = await _playersRepository.GetAllAsync();

            //TODO: use the cards repository instead and update the players after
            foreach (var player in players)
            {
                while (player.Cards.Count < _gameSettings!.NumberOfPlayerCards)
                    await _playersRepository.PlayerDrawCardAsync(player);
            }
        }

        private async Task InitializeCards()
        {
            await _drawPileRepository.DeleteAllAsync();
            await _discardPileRepository.DeleteAllAsync();

            await _drawPileRepository.AddManyRandomAsync(_cardsFactory.GenerateDeck());
            var drawCard = await _drawPileRepository.DrawCardAsync();

            await _discardPileRepository.AddCardAsync(drawCard!);
        }

        private async Task InitializeOnline()
        {
            await GenerateOnlinePlayer();

            var players = await _playersRepository.GetAllAsync();

            await _gameSettingsRepository.WaitGameStartAsync(players.Count);
        }

        private async Task GenerateOnlinePlayer()
        {
            _onlinePlayer = new()
            {
                Id = ObjectId.GenerateNewId(),
                Name = _userCommunicator.GetMessageFromUser("Please enter a name for your player"),
                LastCheckIn = DateTime.UtcNow,
                PlayerAlgorithm = typeof(ManualPlayerAlgorithm).ToString(),
                Cards = [],
                IsPlaying = false
            };

            await _playersRepository.CreateNewAsync(_onlinePlayer);
        }

        private async Task InitializeNormal()
        {
            var players = Enumerable.Range(0, _gameSettings!.NumberOfPlayers)
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

            await DrawCards();
        }
    }
}
