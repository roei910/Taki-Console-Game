﻿using Microsoft.Extensions.DependencyInjection;
using Taki.Game.Deck;
using Taki.Game.Managers;
using Taki.Game.Messages;
using Taki.Game.Players;

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

        public TakiGameGenerator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _userCommunicator = serviceProvider.GetRequiredService<IUserCommunicator>();
            _playersHandlerFactory = serviceProvider.GetRequiredService<PlayersHandlerFactory>();
        }

        internal TakiGameRunner ChooseTypeOfGame()
        {
            ICardDecksHolder cardsHolder = _serviceProvider.GetRequiredService<ICardDecksHolder>();
            int numberOfCards = cardsHolder.CountAllCards();

            GameTypeEnum typeOfGame = _userCommunicator
                .GetEnumFromUser<GameTypeEnum>();

            switch (typeOfGame)
            {
                case GameTypeEnum.Normal:
                    PlayersHolder playerHandler = _playersHandlerFactory
                        .GeneratePlayersHandler(_serviceProvider, numberOfCards);

                    return new TakiGameRunner(playerHandler, _serviceProvider);

                case GameTypeEnum.Pyramid:
                    PlayersHolder pyramidPlayersHandler = _playersHandlerFactory
                        .GeneratePyramidPlayersHandler(_serviceProvider);

                    return new TakiGameRunner(pyramidPlayersHandler, _serviceProvider);

                default:
                    throw new Exception("type enum was wrong");
            }
        }
    }
}
