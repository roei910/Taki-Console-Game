using Microsoft.Extensions.DependencyInjection;
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
                    _userCommunicator.GetCharFromUser($"Winner #{i + 1} is {winner.Name}\n" +
                        $"Press any key to continue");

                    return winner;
                }).ToList();

            if (winners[0].IsManualPlayer())
            {
                _gameScore.SetScoreByName(winners[0].Name, ++winners[0].Score);
                _gameScore.UpdateScores();
            }

            _userCommunicator.SendMessageToUser("The winners by order:");

            winners.Select((winner, i) =>
            {
                _userCommunicator.SendMessageToUser($"{i+1}. {winner.Name}");

                return winner;
            }).ToList();
        }

        public void StartGameLoop()
        {
            if (!ChooseGameType())
                return;

            ResetGame();

            StartSingleGame();

            StartGameLoop();
        }

        private void ResetGame()
        {
            var cards = _playersHolder!.ReturnCardsFromPlayers();
            _cardDecksHolder.ResetCards(cards);
            _playersHolder.ResetPlayers();
            _playersHolder.DealCards(_cardDecksHolder);
        }

        private bool ChooseGameType()
        {
            int numberOfCards = _cardDecksHolder.CountAllCards();

            GameRunnerOptions options = (_playersHolder != null) ?
                    _userCommunicator.GetEnumFromUser<GameRunnerOptions>() :
                    _userCommunicator.GetEnumFromUser(new List<GameRunnerOptions>() { GameRunnerOptions.RestartGame });

            switch (options)
            {
                case GameRunnerOptions.NewNormalGame:
                    _playersHolder = _playersHolderFactory
                    .GeneratePlayersHandler(numberOfCards);
                    break;

                case GameRunnerOptions.NewPyramidGame:
                    _playersHolder = _playersHolderFactory
                    .GeneratePyramidPlayersHandler();
                    break;

                case GameRunnerOptions.RestartGame:
                    break;

                case GameRunnerOptions.QuitGame:
                    return false;

                default:
                    throw new Exception("type enum was invalid");
            }

            return true;
        }
    }
}
