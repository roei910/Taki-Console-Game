﻿using MongoDB.Bson;
using Taki.Models.Algorithm;
using TakiApp.Shared.Interfaces;
using TakiApp.Shared.Models;

namespace TakiApp.Services.GameLogic
{
    //TODO: check the initializer and restore functionality maybe create classes for them
    //TODO: create tasks for computer players => they will run seperately and know when to play 
    public class GameInitializer : IGameInitializer
    {
        private readonly IUserCommunicator _userCommunicator;
        private readonly IGameSettingsRepository _gameSettingsRepository;
        private readonly IPlayersRepository _playersRepository;
        private readonly IDrawPileRepository _drawPileRepository;
        private readonly IDiscardPileRepository _discardPileRepository;
        private readonly ConstantVariables _constantVariables;
        private readonly ICardPlayService _cardPlayService;
        private readonly IComputerPlayersRunner _computerPlayersRunner;
        private GameSettings? _gameSettings;
        private Player? _onlinePlayer;

        public Player GetPlayer => _onlinePlayer!;

        public GameInitializer(IUserCommunicator userCommunicator, IGameSettingsRepository gameSettingsRepository,
            IPlayersRepository playersRepository, IDrawPileRepository drawPileRepository, 
            IDiscardPileRepository discardPileRepository, ConstantVariables constantVariables,
            ICardPlayService cardPlayService, IComputerPlayersRunner computerPlayersRunner)
        {
            _userCommunicator = userCommunicator;
            _gameSettingsRepository = gameSettingsRepository;
            _playersRepository = playersRepository;
            _drawPileRepository = drawPileRepository;
            _discardPileRepository = discardPileRepository;
            _constantVariables = constantVariables;
            _cardPlayService = cardPlayService;
            _computerPlayersRunner = computerPlayersRunner;
        }

        public GameSettings GetGameSettings()
        {
            return _gameSettings!;
        }

        public async Task<bool> IsGameInitializedAsync()
        {
            _gameSettings = await _gameSettingsRepository.GetGameSettingsAsync();

            return _gameSettings is not null;
        }

        public async Task InitializeGame()
        {
            _gameSettings = await _gameSettingsRepository.GetGameSettingsAsync();

            if(_gameSettings == null)
            {
                await InitializeNewGame();

                return;
            }

            if (_gameSettings!.HasGameStarted)
            {
                await HandleGameStarted();

                return;
            }

            await InitializeOnlinePlayer();
        }

        private async Task InitializeNewGame()
        {
            await CreateNewGameSettings();

            await InitializeCards();

            if (_gameSettings!.IsOnline)
            {
                await InitializeOnlinePlayer();
                //await GenerateOnlinePlayer();
                //await _gameSettingsRepository.WaitGameStartAsync(1);
                await DrawCards();

                return;
            }

            await InitializeNormal();
        }

        private async Task CreateNewGameSettings()
        {
            _userCommunicator.SendMessageToUser("Building a new Taki Game!");

            _userCommunicator.SendMessageToUser("Please enter type of game: online or normal");
            var isOnline = _userCommunicator.UserPickItemFromList(new List<string>() { "online", "normal" });

            //_userCommunicator.SendMessageToUser("Please enter normal or pyramid");
            //var typeOfGame = _userCommunicator.UserPickItemFromList(new List<string>() { "normal", "pyramid" });

            var numberOfPlayers = _userCommunicator.GetNumberFromUser("Please enter number of players",
                _constantVariables.MinNumberOfPlayers, _constantVariables.MaxNumberOfPlayers);

            var numberOfManualPlayers = _userCommunicator.GetNumberFromUser("Please enter number of manual players",
                1, numberOfPlayers);

            int maxNumberOfPlayerCards = _cardPlayService.GenerateCardsDeck().Count / numberOfPlayers - 1;

            var maxCards = maxNumberOfPlayerCards > _constantVariables.MaxNumberOfPlayerCards ?
                _constantVariables.MaxNumberOfPlayerCards : maxNumberOfPlayerCards;

            var numberOfPlayerCards = _userCommunicator.GetNumberFromUser("Please enter number of player cards",
                _constantVariables.MinNumberOfPlayerCards, maxCards);

            _gameSettings = new GameSettings()
            {
                Id = ObjectId.GenerateNewId(),
                HasGameStarted = false,
                IsOnline = isOnline == "online",
                NumberOfPlayerCards = numberOfPlayerCards,
                //TypeOfGame = typeOfGame,
                NumberOfPlayers = numberOfPlayers,
                NumberOfManualPlayers = numberOfManualPlayers,
                NumberOfWinners = _constantVariables.NumberOfTotalWinners
            };

            await _gameSettingsRepository.CreateGameSettings(_gameSettings);
        }

        private async Task HandleGameStarted()
        {
            _userCommunicator.SendErrorMessage("Game already started, would you like to create a new game?");

            var choices = new List<string>() { "yes", "no" };

            if (!_gameSettings!.IsOnline)
                choices.Add("continue previous game");

            string answer = _userCommunicator.UserPickItemFromList(choices);

            if (answer == "no")
            {
                _userCommunicator.SendErrorMessage("GoodBye!");
                return;
            }

            if (answer == "continue previous game")
            {
                //TODO: continue the previous offline game

                return;
            }

            await _gameSettingsRepository.DeleteGameAsync();

            await InitializeGame();
        }

        private async Task DrawCards()
        {
            var players = await _playersRepository.GetAllAsync();

            var cards = await _drawPileRepository.DrawCardsAsync(players.Count * _gameSettings!.NumberOfPlayerCards);

            while(cards.Count > 0)
            {
                foreach (var player in players) 
                {
                    player.Cards.Add(cards[0]);
                    cards.RemoveAt(0);
                }
            }

            await _playersRepository.UpdateManyAsync(players);
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
            var players = await _playersRepository.GetAllAsync();

            if(players.Count < _gameSettings!.NumberOfManualPlayers)
            {
                var player = await GenerateOnlinePlayer();
                players.Add(player);
            }

            if (players.Count == _gameSettings!.NumberOfManualPlayers)
            {
                var computerPlayers = await _computerPlayersRunner.GenerateComputerPlayers();

                players.AddRange(computerPlayers);
            }

            await _gameSettingsRepository.WaitGameStartAsync(players.Count);
        }

        private async Task<Player> GenerateOnlinePlayer()
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

            return _onlinePlayer;
        }

        private async Task InitializeNormal()
        {
            var players = Enumerable.Range(0, _gameSettings!.NumberOfManualPlayers)
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

            var numberOfComputerPlayers = _gameSettings!.NumberOfPlayers - _gameSettings!.NumberOfManualPlayers;

            var computerPlayers = GenerateComputerPlayers(numberOfComputerPlayers);

            players.AddRange(computerPlayers);

            await _playersRepository.CreateManyAsync(players);

            await DrawCards();

            await _gameSettingsRepository.WaitGameStartAsync(players.Count);
        }       

        private List<Player> GenerateComputerPlayers(int numberOfPlayer)
        {
            var players = Enumerable.Range(0, numberOfPlayer)
                .Select(x =>
                {
                    Player player = new Player()
                    {
                        Id = ObjectId.GenerateNewId(),
                        Name = $"Player {x + 1}",
                        LastCheckIn = DateTime.UtcNow,
                        PlayerAlgorithm = typeof(PlayerAlgorithm).ToString(),
                        Cards = [],
                        IsPlaying = false
                    };

                    return player;
                }).ToList();

            return players;
        }
    }
}
