using Microsoft.Extensions.DependencyInjection;
using Taki.Game.GameRules;
using Taki.Game.Handlers;
using Taki.Game.Interfaces;
using Taki.Game.Managers;

namespace Taki.Game.Factories
{
    enum GameTypeEnum
    {
        Normal,
        Pyramid
    }

    internal class TakiGameFactory
    {
        //TODO: move all consts from code
        private const int MIN_NUMBER_OF_PLAYER_CARDS = 7;
        private const int MAX_NUMBER_OF_PLAYER_CARDS = 20;

        private readonly IMessageHandler _messageHandler;
        private readonly IServiceProvider _serviceProvider;

        public TakiGameFactory(IServiceProvider serviceProvider)
        {
            _messageHandler = serviceProvider.GetRequiredService<IMessageHandler>();
            _serviceProvider = serviceProvider;
        }

        internal TakiGameRunner ChooseTypeOfGame(PlayersHandlerFactory playersHandlerFactory, CardsHandlerFactory cardsHandlerFactory, ServiceProvider serviceProvider)
        {
            CardsHandler cardsHandler = cardsHandlerFactory.GenerateCardsHandler();

            GameTypeEnum typeOfGame = _messageHandler
                .GetEnumFromUser<GameTypeEnum>();

            switch (typeOfGame)
            {
                case GameTypeEnum.Normal:
                    PlayersHandler playerHandler = playersHandlerFactory.GeneratePlayersHandler();
                    GameHandlers gameHandlers = new(playerHandler, cardsHandler, serviceProvider);

                    return new TakiGameRunner(gameHandlers);

                case GameTypeEnum.Pyramid:
                    PlayersHandler pyramidPlayersHandler = playersHandlerFactory.GeneratePyramidPlayersHandler();
                    GameHandlers pyramidGameHandlers = new(pyramidPlayersHandler, cardsHandler, serviceProvider);
                    
                    return new TakiGameRunner(pyramidGameHandlers);

                default:
                    throw new Exception("type enum was wrong");
            }
        }

        
    }
}
