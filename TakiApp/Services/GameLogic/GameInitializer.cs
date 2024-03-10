using MongoDB.Bson;
using Taki.Models.Algorithm;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Services.GameLogic
{
    public class GameInitializer : IGameInitializer
    {
        private readonly IUserCommunicator _userCommunicator;
        private readonly IGameSettingsRepository _gameSettingsRepository;
        private readonly IPlayersRepository _playersRepository;
        private readonly IDrawPileRepository _drawPileRepository;
        private readonly IDiscardPileRepository _discardPileRepository;
        private readonly ConstantVariables _constantVariables;
        private readonly ICardPlayService _cardPlayService;
        private GameSettings? _gameSettings;
        private Player? _onlinePlayer;

        public Player GetPlayer => _onlinePlayer!;

        public GameInitializer(IUserCommunicator userCommunicator, IGameSettingsRepository gameSettingsRepository,
            IPlayersRepository playersRepository, IDrawPileRepository drawPileRepository, 
            IDiscardPileRepository discardPileRepository, ConstantVariables constantVariables,
            ICardPlayService cardPlayService)
        {
            _userCommunicator = userCommunicator;
            _gameSettingsRepository = gameSettingsRepository;
            _playersRepository = playersRepository;
            _drawPileRepository = drawPileRepository;
            _discardPileRepository = discardPileRepository;
            _constantVariables = constantVariables;
            _cardPlayService = cardPlayService;
        }

        public GameSettings GetGameSettings()
        {
            return _gameSettings!;
        }

        public async Task InitializeGame()
        {
            _gameSettings = await _gameSettingsRepository.GetGameSettingsAsync();

            if(_gameSettings == null)
            {
                _userCommunicator.SendMessageToUser("Building a new Taki Game!");

                _userCommunicator.SendMessageToUser("Please enter type of game: online or normal");
                var isOnline = _userCommunicator.UserPickItemFromList(new List<string>() { "online", "normal"});

                _userCommunicator.SendMessageToUser("Please enter normal or pyramid");
                var typeOfGame = _userCommunicator.UserPickItemFromList(new List<string>() { "normal", "pyramid" });

                var numberOfPlayerCards = _userCommunicator.GetNumberFromUser("Please enter number of player cards", 
                    _constantVariables.MinNumberOfPlayerCards, _constantVariables.MaxNumberOfPlayerCards);

                var numberOfPlayers = _userCommunicator.GetNumberFromUser("Please enter number of players", 
                    _constantVariables.MinNumberOfPlayers, _constantVariables.MaxNumberOfPlayers);

                _gameSettings = new GameSettings()
                {
                    Id = ObjectId.GenerateNewId(),
                    HasGameStarted = false,
                    IsOnline = isOnline == "online",
                    NumberOfPlayerCards = numberOfPlayerCards,
                    TypeOfGame = typeOfGame,
                    NumberOfPlayers = numberOfPlayers,
                    NumberOfWinners = _constantVariables.NumberOfTotalWinners
                };

                await _gameSettingsRepository.CreateGameSettings(_gameSettings);

                await InitializeCards();

                //TODO: fix not allowing normal game

                if (_gameSettings.IsOnline)
                {
                    await GenerateOnlinePlayer();
                    await _gameSettingsRepository.WaitGameStartAsync(1);
                    await DrawCards();

                    return;
                }

                await InitializeNormal();

                return;
            }

            if (_gameSettings!.HasGameStarted)
            {
                _userCommunicator.SendErrorMessage("Game already started, would you like to create a new game?");

                string answer = _userCommunicator.UserPickItemFromList(new List<string>() { "yes", "no" });

                if(answer == "no")
                {
                    _userCommunicator.SendErrorMessage("GoodBye!");
                    return;
                }

                await _gameSettingsRepository.DeleteGameAsync();

                await InitializeGame();

                return;
            }

            await InitializeOnlinePlayer();
        }

        private async Task DrawCards()
        {
            var players = await _playersRepository.GetAllAsync();

            //TODO: use the cards repository instead and update the players after
            foreach (var player in players)
                await _playersRepository.DrawCardsAsync(player, _gameSettings!.NumberOfPlayerCards);
        }

        private async Task InitializeCards()
        {
            await _drawPileRepository.DeleteAllAsync();
            await _discardPileRepository.DeleteAllAsync();

            await _drawPileRepository.ShuffleCardsAsync(_cardPlayService.GenerateCardsDeck());
            var drawCard = await _drawPileRepository.DrawCardsAsync();

            await _discardPileRepository.AddCardOrderedAsync(drawCard!.First());
        }

        private async Task InitializeOnlinePlayer()
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

            await _playersRepository.CreatePlayerAsync(_onlinePlayer);
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

            await _gameSettingsRepository.WaitGameStartAsync(players.Count);
        }
    }
}
