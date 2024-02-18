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
        protected readonly IUserCommunicator _userCommunicator;
        protected readonly ICardDecksHolder _cardsHolder;
        protected readonly ProgramVariables _programVariables;
        protected readonly IServiceProvider _serviceProvider;
        protected IPlayersHolder? _playersHolder;

        public TakiGameRunner(IServiceProvider serviceProvider)
        {
            _cardsHolder = serviceProvider.GetRequiredService<ICardDecksHolder>();
            _userCommunicator = serviceProvider.GetRequiredService<IUserCommunicator>();
            _programVariables = serviceProvider.GetRequiredService<ProgramVariables>();
            _serviceProvider = serviceProvider;
        }

        private void StartSingleGame()
        {
            var gameScores = _serviceProvider.GetRequiredService<IGameScore>();

            int numOfPlayers = _playersHolder!.Players.Count;
            int totalWinners = _programVariables.NUMBER_OF_TOTAL_WINNERS;

            var winners = Enumerable.Range(0,
                totalWinners > numOfPlayers ? numOfPlayers : totalWinners)
                .Select(i =>
                {
                    Player winner = _playersHolder.GetWinner();
                    _userCommunicator.GetCharFromUser($"Winner #{i + 1} is {winner.Name}\n" +
                        $"Press any key to continue");

                    return winner;
                }).ToList();

            if (winners[0].IsManualPlayer())
            {
                gameScores.SetScoreByName(winners[0].Name, ++winners[0].Score);
                gameScores.UpdateScores();
            }

            _userCommunicator.SendMessageToUser("The winners by order:");

            winners.Select(winner =>
            {
                _userCommunicator.SendMessageToUser($"{winners.IndexOf(winner)}. {winner.Name}");

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
            _cardsHolder.ResetCards(cards);
            _playersHolder.ResetPlayers();
            _playersHolder.DealCards(_cardsHolder);
        }

        private bool ChooseGameType()
        {
            PlayersHolderFactory playersHolderFactory = _serviceProvider.GetRequiredService<PlayersHolderFactory>();
            int numberOfCards = _cardsHolder.CountAllCards();

            GameRunnerOptions options = (_playersHolder != null) ?
                    _userCommunicator.GetEnumFromUser<GameRunnerOptions>() :
                    _userCommunicator.GetEnumFromUser(new List<GameRunnerOptions>() { GameRunnerOptions.RestartGame });

            switch (options)
            {
                case GameRunnerOptions.NewNormalGame:
                    _playersHolder = playersHolderFactory
                    .GeneratePlayersHandler(_serviceProvider, numberOfCards);
                    break;

                case GameRunnerOptions.NewPyramidGame:
                    _playersHolder = playersHolderFactory
                    .GeneratePyramidPlayersHandler(_serviceProvider);
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
