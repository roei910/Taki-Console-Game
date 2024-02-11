﻿using Microsoft.Extensions.DependencyInjection;
using Taki.Game.Cards;
using Taki.Game.GameRules;
using Taki.Game.Handlers;
using Taki.Game.Interfaces;
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
        private const int NUMBER_OF_PYRAMID_PLAYER_CARDS = 10;

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

        public TakiGameRunner(GameHandlers handlers, IServiceProvider serviceProvider)
        {
            var players = handlers.GetPlayersHandler().GetAllPlayers();
            _gameHandlers = handlers;
            _playersHandler = new PyramidPlayersHandler(players);
            _cardsHandler = handlers.GetCardsHandler();
            _messageHandler = serviceProvider.GetRequiredService<IMessageHandler>();
            _numberOfPlayerCards = NUMBER_OF_PYRAMID_PLAYER_CARDS;
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
            winners.ForEach(winner => _messageHandler.SendMessageToUser($"{winners.IndexOf(winner)}. {winner.Name}"));
        }

        private void ResetGame()
        {
            _playersHandler.ResetPlayers(_cardsHandler);
            _cardsHandler.ResetCards();
            DealCards();
        }

        private void DealCards()
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

        private Player GetWinner()
        {
            while (_playersHandler.CanCurrentPlayerPlay())
                _playersHandler.CurrentPlayerPlay(_gameHandlers);

            return _playersHandler.RemoveWinner();
        }
    }
}