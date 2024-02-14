using Microsoft.Extensions.DependencyInjection;
using Taki.Game.GameRunner;
using Taki.Game.Handlers;
using Taki.Game.Messages;
using Taki.Game.Players;

namespace Taki.Game.Managers
{
    internal class TakiGameRunner : ITakiGameRunner
    {
        protected readonly IUserCommunicator _userCommunicator;
        protected readonly IPlayersHandler _playersHandler;
        protected readonly ICardsHandler _cardsHandler;
        protected readonly ProgramVariables _programVariables;
        protected readonly IServiceProvider _serviceProvider;

        public TakiGameRunner(IPlayersHandler playersHandler, IServiceProvider serviceProvider)
        {
            _playersHandler = playersHandler;
            _cardsHandler = serviceProvider.GetRequiredService<ICardsHandler>();
            _userCommunicator = serviceProvider.GetRequiredService<IUserCommunicator>();
            _programVariables = serviceProvider.GetRequiredService<ProgramVariables>();
            _serviceProvider = serviceProvider;
        }

        public void StartGame()
        {
            ResetGame();

            int numOfPlayers = _playersHandler.CountPlayers();
            int totalWinners = _programVariables.NUMBER_OF_TOTAL_WINNERS;

            var winners = Enumerable.Range(0,
                totalWinners > numOfPlayers ? numOfPlayers : totalWinners)
                .Select(i =>
                {
                    Player winner = GetWinner();
                    _userCommunicator.GetCharFromUser($"Winner #{i + 1} is {winner.Name}\n" +
                        $"Press any key to continue");
                    return winner;
                }).ToList();

            _userCommunicator.SendMessageToUser("The winners by order:");

            winners.Select(winner =>
            {
                _userCommunicator.SendMessageToUser($"{winners.IndexOf(winner)}. {winner.Name}");
                return winner;
            }).ToList();
        }

        public void ResetGame()
        {
            var cards = _playersHandler.GetAllCardsFromPlayers();
            _cardsHandler.ResetCards(cards);
            _playersHandler.DealCards(_cardsHandler);
        }

        private Player GetWinner()
        {
            while (_playersHandler.HasPlayerWon())
                _playersHandler.CurrentPlayerPlay(_serviceProvider);

            return _playersHandler.RemoveWinner();
        }
    }
}
