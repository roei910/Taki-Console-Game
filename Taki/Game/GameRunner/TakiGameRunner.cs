using Taki.Game.Deck;
using Taki.Game.Factories;
using Taki.Game.GameRunner;
using Taki.Game.Messages;
using Taki.Game.Players;

namespace Taki.Game.Managers
{
    enum GameRunnerOptions
    {
        NewNormalGame,
        NewPyramidGame,
        RestartGame,
        ShowAllScores,
        QuitGame
    }

    internal class TakiGameRunner : ITakiGameRunner
    {
        protected readonly PlayersHolderFactory _playersHolderFactory;
        protected readonly IUserCommunicator _userCommunicator;
        protected readonly ICardDecksHolder _cardDecksHolder;
        protected readonly ProgramVariables _programVariables;
        protected readonly IGameScore _gameScore;
        protected IPlayersHolder? _playersHolder;

        public TakiGameRunner(PlayersHolderFactory playersHolderFactory, CardDeckFactory cardDeckFactory,
            IUserCommunicator userCommunicator, ProgramVariables programVariables, IGameScore gameScore, Random random)
        {
            _userCommunicator = userCommunicator;
            _programVariables = programVariables;
            _playersHolderFactory = playersHolderFactory;
            _gameScore = gameScore;
            _cardDecksHolder = new CardDecksHolder(cardDeckFactory, random);
        }

        private void StartSingleGame()
        {
            int numOfPlayers = _playersHolder!.Players.Count;
            int totalWinners = _programVariables.NUMBER_OF_TOTAL_WINNERS;

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
                _userCommunicator.SendMessageToUser($"{i+1}. {winner.Name}");

                return winner;
            }).ToList();

            _userCommunicator.SendMessageToUser();
        }

        public void StartGameLoop()
        {
            if (!ChooseGameType())
                return;

            if (_playersHolder is not null)
            {
                ResetGame();
                _playersHolder!.DealCards(_cardDecksHolder);
                StartSingleGame();
            }

            StartGameLoop();
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

            GameRunnerOptions options = (_playersHolder is not null) ?
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
