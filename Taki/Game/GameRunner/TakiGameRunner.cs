using Taki.Game.GameRules;
using Taki.Game.Handlers;
using Taki.Game.Interfaces;
using Taki.Game.Players;

//TODO: challenge: don't use any loops (except the main game while) in the code
//TODO: const variables to configurations and keep consistency of readonlies/consts 
//TODO: get objects from dependency injection

namespace Taki.Game.Managers
{
    internal class TakiGameRunner
    {
        private const int NUMBER_OF_TOTAL_WINNERS = 2;

        protected GameHandlers _gameHandlers;
        protected IMessageHandler _messageHandler;
        protected readonly PlayersHandler _playersHandler;
        protected readonly CardsHandler _cardsHandler;

        public TakiGameRunner(GameHandlers handlers)
        {
            _gameHandlers = handlers;
            _playersHandler = handlers.GetPlayersHandler();
            _cardsHandler = handlers.GetCardsHandler();
            _messageHandler = handlers.GetMessageHandler();
        }

        public void StartGame()
        {
            ResetGame();

            var numOfPlayers = _playersHandler.GetAllPlayers().Count;

            var winners = Enumerable.Range(0, 
                NUMBER_OF_TOTAL_WINNERS > numOfPlayers ? 
                numOfPlayers : NUMBER_OF_TOTAL_WINNERS)
                .Select(i =>
                {
                    Player winner = GetWinner();
                    _messageHandler.SendMessageToUser($"Winner #{i + 1} is {winner.Name}");
                    _messageHandler.SendMessageToUser("Press any key to continue");
                    _messageHandler.GetMessageFromUser();
                    return winner;
                }).ToList();

            _messageHandler.SendMessageToUser("The winners by order:");
            winners.Select(winner =>
            {
                _messageHandler.SendMessageToUser($"{winners.IndexOf(winner)}. {winner.Name}");
                return winner;
            }).ToList();
        }

        private void ResetGame()
        {
            //TODO: change to get all cards and put them in the deck from here
            _playersHandler.ResetPlayers(_cardsHandler);
            _cardsHandler.ResetCards();
            _playersHandler.DealCards(_cardsHandler);
        }

        

        private Player GetWinner()
        {
            while (_playersHandler.CanCurrentPlayerPlay())
                _playersHandler.CurrentPlayerPlay(_gameHandlers);

            return _playersHandler.RemoveWinner();
        }
    }
}
