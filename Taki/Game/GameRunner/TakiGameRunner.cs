using Microsoft.Extensions.DependencyInjection;
using Taki.Game.Deck;
using Taki.Game.GameRunner;
using Taki.Game.Messages;
using Taki.Game.Players;

namespace Taki.Game.Managers
{
    internal class TakiGameRunner : ITakiGameRunner
    {
        protected readonly IUserCommunicator _userCommunicator;
        protected readonly IPlayersHolder _playersHolder;
        protected readonly ICardDecksHolder _cardsHolder;
        protected readonly ProgramVariables _programVariables;
        protected readonly IServiceProvider _serviceProvider;

        public TakiGameRunner(IPlayersHolder playersHolder, IServiceProvider serviceProvider)
        {
            _playersHolder = playersHolder;
            _cardsHolder = serviceProvider.GetRequiredService<ICardDecksHolder>();
            _userCommunicator = serviceProvider.GetRequiredService<IUserCommunicator>();
            _programVariables = serviceProvider.GetRequiredService<ProgramVariables>();
            _serviceProvider = serviceProvider;
        }

        public void StartGame()
        {
            ResetGame();
            var gameScores = _serviceProvider.GetRequiredService<IGameScore>();

            int numOfPlayers = _playersHolder.Players.Count;
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

        private void ResetGame()
        {
            var cards = _playersHolder.ReturnCardsFromPlayers();
            _cardsHolder.ResetCards(cards);
            _playersHolder.ResetPlayers();
            _playersHolder.DealCards(_cardsHolder);
        }
    }
}
