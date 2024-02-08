using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using Taki.Game.Algorithm;
using Taki.Game.Cards;
using Taki.Game.Communicators;
using Taki.Game.Deck;
using Taki.Game.GameRules;
using Taki.Game.Handlers;
using Taki.Game.Players;

//TODO: challenge: don't use any loops (except the main game while) in the code
//TODO: rebalance factory and manager
//TODO: const variables to configurations and keep consistency of readonlies/consts 
//TODO: get objects from dependency injection
//TODO: remove pyramidGameManager, just get the right rule as a parameter from the factory/runner/DI/program
//TODO: folders hierarchy
//TODO: No comments
//TODO: Don't leave unreachable code


namespace Taki.Game.Managers
{
    internal class TakiGameRunner
    {
        private const int NUMBER_OF_TOTAL_WINNERS = 2;

        protected GameHandlers _gameHandlers;
        protected IMessageHandler _messageHandler;
        protected readonly PlayersHandler _playersHandler;
        protected readonly CardsHandler _cardsHandler;
        protected int _numberOfPlayerCards;

        public TakiGameRunner(GameHandlers handlers, IServiceProvider serviceProvider, int numberOfPlayerCards)
        {
            _gameHandlers = handlers;
            _playersHandler = handlers.GetPlayersHandler();
            _cardsHandler = handlers.GetCardsHandler();
            _messageHandler = serviceProvider.GetRequiredService<IMessageHandler>();
            _numberOfPlayerCards = numberOfPlayerCards;
        }

        public void StartGame()
        {
            ResetGame();

            //TODO: handle what happends when number of winners is higher than number of players
            var winners = Enumerable.Range(0, NUMBER_OF_TOTAL_WINNERS)
                .Select(i =>
                {
                    Player winner = GetWinner();
                    _messageHandler.SendMessageToUser($"Winner #{i + 1} is {winner.Name}");
                    _messageHandler.SendMessageToUser("Press any key to continue");
                    _messageHandler.GetMessageFromUser();
                    return winner;
                }).ToList();

            _messageHandler.SendMessageToUser("The winners by order:");
            winners.ForEach(winner => _messageHandler.SendMessageToUser($"{winners.IndexOf(winner)}. {winner.Name}"));
        }

        private void ResetGame()
        {
            //reset players after being removed
            _cardsHandler.ResetCards();
            DealCards();
        }

        //from itzhaki - move to rule handler / pyramid rule handler - they need to control the first draw of cards
        //try to make pyramid here
        internal void DealCards()
        {
            Enumerable.Range(0, _numberOfPlayerCards).ToList()
                .ForEach(i =>
                {
                    _playersHandler.GetAllPlayers()
                    .ForEach(p =>
                    {
                        Card drawCard = _cardsHandler.DrawCard();
                        p.AddCard(drawCard);
                    });
                });

            _cardsHandler.DrawFirstCard();
        }

        public Player GetWinner()
        {
            while (_playersHandler.CanCurrentPlayerPlay())
                _playersHandler.CurrentPlayerPlay(_gameHandlers);

            return _playersHandler.RemoveWinner();
        }
    }
}
