﻿using Microsoft.Extensions.DependencyInjection;
using Taki.Game.GameRules;
using Taki.Game.Managers;
using Taki.Game.Messages;

namespace Taki.Game.Factories
{
    enum GameTypeEnum
    {
        Normal,
        Pyramid
    }

    internal class TakiGameGenerator
    {
        private readonly IUserCommunicator _userCommunicator;
        private readonly IServiceProvider _serviceProvider;
        private readonly PlayersHandlerFactory _playersHandlerFactory;
        private readonly CardsHandlerFactory _cardsHandlerFactory;

        public TakiGameGenerator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _userCommunicator = serviceProvider.GetRequiredService<IUserCommunicator>();
            _playersHandlerFactory = serviceProvider.GetRequiredService<PlayersHandlerFactory>();
            _cardsHandlerFactory = serviceProvider.GetRequiredService<CardsHandlerFactory>();
        }

        internal TakiGameRunner ChooseTypeOfGame()
        {
            CardsHandler cardsHandler = _cardsHandlerFactory.GenerateCardsHandler(_serviceProvider);
            int numberOfCards = cardsHandler.CountAllCards();

            GameTypeEnum typeOfGame = _userCommunicator
                .GetEnumFromUser<GameTypeEnum>();

            switch (typeOfGame)
            {
                case GameTypeEnum.Normal:
                    PlayersHandler playerHandler = _playersHandlerFactory
                        .GeneratePlayersHandler(_serviceProvider, numberOfCards);

                    return new TakiGameRunner(playerHandler, cardsHandler, _serviceProvider);

                case GameTypeEnum.Pyramid:
                    PlayersHandler pyramidPlayersHandler = _playersHandlerFactory
                        .GeneratePyramidPlayersHandler(_serviceProvider);

                    return new TakiGameRunner(pyramidPlayersHandler, cardsHandler, _serviceProvider);

                default:
                    throw new Exception("type enum was wrong");
            }
        }
    }
}
