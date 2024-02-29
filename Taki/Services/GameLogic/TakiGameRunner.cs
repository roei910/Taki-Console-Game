using MongoDB.Driver;
using Taki.Factories;
using Taki.Models.Deck;
using Taki.Models.Players;
using Taki.Shared.Enums;
using Taki.Shared.Interfaces;
using Taki.Shared.Models;

namespace Taki.Models.GameLogic
{
    public class TakiGameRunner : ITakiGameRunner
    {
        protected readonly PlayersHolderFactory _playersHolderFactory;
        protected readonly IUserCommunicator _userCommunicator;
        protected readonly ICardDecksHolder _cardDecksHolder;
        protected readonly ConstantVariables _constantVariables;
        protected readonly IGameScore _gameScore;
        private readonly GameRestore _gameRestore;
        protected IPlayersHolder? _playersHolder;

        public TakiGameRunner(PlayersHolderFactory playersHolderFactory, 
            IUserCommunicator userCommunicator, ConstantVariables constantVariables, 
            IGameScore gameScore, GameRestore gameRestore, 
            CardDecksHolder cardDecksHolder)
        {
            _userCommunicator = userCommunicator;
            _constantVariables = constantVariables;
            _playersHolderFactory = playersHolderFactory;
            _gameScore = gameScore;
            _gameRestore = gameRestore;
            _cardDecksHolder = cardDecksHolder;
        }

        public void StartGameLoop()
        {
            //TODO: check if a game exists in mongodb locally
            //TODO: if the game exists then choose a name and connect the user.
            //TODO: check what happends when someone exists and doesnt come back to the game
            //TODO: if there isnt a game already open the computer asks to create a new local or multi computer game
            //TODO: when someone connects to the game other people will get a message saying someone joined the game

            //TODO: Class to hanle turns
            //TODO: we need to handle the turns for the players, check the first person in the list to know if it is our turn or not.
            //TODO: if a player finished his play we need to add in our screen what happened.
            //TODO: when someone plays the mongo gets updated from that person and he goes to the next player.
            //TODO: handle the game only from mongodb, we dont need to keep models anymore
            

            //TODO: extract functionality from MODELS

            //TODO: class to handle screen updates

            //TODO: if we stop a game in the middle we need to know what to do with it to continue the game, choose your name from the list

            //TODO: handle the cards with services that get the card from the db and know how to handle the next turn
            //TODO: the card service will get the information to know what to do next, if taki is open or something.
            //TODO: create a function to know which card it needs to use (MatchCardService??)

            //TODO: mongo acces will be async now
            if (_gameRestore.TryRestoreTakiGame(_cardDecksHolder, out _playersHolder))
                StartSingleGame();

            if (!ChooseGameType())
                return;

            if (_playersHolder is not null)
            {
                GameSettings gameSettings = new GameSettings()
                {
                    NumberOfPlayerCards = _playersHolder.NumberOfPlayerCards,
                    TypeOfGame = _playersHolder.GetType().Name
                };

                _gameRestore.CreateGameSettings(gameSettings);

                ResetGame();
                _playersHolder!.DealCards(_cardDecksHolder);
                _cardDecksHolder.DrawFirstCard();
                StartSingleGame();
            }

            StartGameLoop();
        }

        private void StartSingleGame()
        {
            int numOfPlayers = _playersHolder!.Players.Count;
            int totalWinners = _constantVariables.NumberOfTotalWinners;

            var winners = Enumerable.Range(0,
                totalWinners > numOfPlayers ? numOfPlayers : totalWinners)
                .Select(i =>
                {
                    Player winner = _playersHolder.GetWinner(_cardDecksHolder);
                    _userCommunicator.GetMessageFromUser($"Winner #{i + 1} is {winner.Name}\n" +
                        $"Press enter to continue");

                    return winner;
                }).ToList();

            if (winners[0].IsManualPlayer())
            {
                _gameScore.SetScoreByName(winners[0].Name, ++winners[0].Score);
                _gameScore.UpdateScoresFile();
            }

            _userCommunicator.SendMessageToUser("The winners by order:");

            winners.Select((winner, i) =>
            {
                _userCommunicator.SendMessageToUser($"{i + 1}. {winner.Name}");

                return winner;
            }).ToList();

            _userCommunicator.SendMessageToUser();
            _gameRestore.DeleteAll();
        }

        private void ResetGame()
        {
            if (_playersHolder is null)
                return;
            var cards = _playersHolder!.ReturnCardsFromPlayers();
            _cardDecksHolder.ResetCards(cards);
            _playersHolder.ResetPlayers();
        }

        private bool ChooseGameType()
        {
            int numberOfCards = _cardDecksHolder.CountAllCards();

            GameRunnerOptions options = _playersHolder is not null ?
                    _userCommunicator.GetEnumFromUser<GameRunnerOptions>() :
                    _userCommunicator.GetEnumFromUser(new List<GameRunnerOptions>() { GameRunnerOptions.RestartGame });

            switch (options)
            {
                case GameRunnerOptions.NewNormalGame:
                    ResetGame();
                    _playersHolder = _playersHolderFactory
                    .GeneratePlayersHandler(numberOfCards);
                    break;

                case GameRunnerOptions.NewPyramidGame:
                    ResetGame();
                    _playersHolder = _playersHolderFactory
                    .GeneratePyramidPlayersHandler();
                    break;

                case GameRunnerOptions.RestartGame:
                    break;

                case GameRunnerOptions.QuitGame:
                    return false;

                case GameRunnerOptions.ShowAllScores:
                    _userCommunicator.SendAlertMessage("The scores are:");
                    _userCommunicator.SendMessageToUser(_gameScore.GetAllScores());
                    _userCommunicator.SendMessageToUser();
                    return ChooseGameType();

                default:
                    throw new Exception("type enum was invalid");
            }

            return true;
        }
    }
}
