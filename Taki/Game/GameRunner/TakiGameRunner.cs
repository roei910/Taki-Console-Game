using Microsoft.Extensions.DependencyInjection;
using Taki.Game.GameRules;
using Taki.Game.GameRunner;
using Taki.Game.Handlers;
using Taki.Game.Messages;
using Taki.Game.Players;

namespace Taki.Game.Managers
{
    internal class TakiGameRunner : ITakiGameRunner
    {
        protected IUserCommunicator _userCommunicator;
        protected readonly PlayersHandler _playersHandler;
        protected readonly CardsHandler _cardsHandler;
        protected readonly ProgramVariables _programVariables;

        public TakiGameRunner(PlayersHandler playersHandler, CardsHandler cardsHandler, IServiceProvider serviceProvider)
        {
            _playersHandler = playersHandler;
            _cardsHandler = cardsHandler;
            _userCommunicator = serviceProvider.GetRequiredService<IUserCommunicator>();
            _programVariables = serviceProvider.GetRequiredService<ProgramVariables>();
        }

        public void StartGame()
        {
            ResetGame();

            int numOfPlayers = _playersHandler._players.Count;
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
            //TODO: no need for cardsHandler
            var cards = _playersHandler.GetAllCardsFromPlayers(_cardsHandler);
            _cardsHandler.ResetCards(cards);
            _playersHandler.DealCards(_cardsHandler);
        }

        private Player GetWinner()
        {
            while (_playersHandler.HasPlayerWon())
                _playersHandler.CurrentPlayerPlay(_cardsHandler, _userCommunicator);

            return _playersHandler.RemoveWinner();
        }
    }
}
