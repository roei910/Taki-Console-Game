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
        private readonly IMessageHandler _messageHandler;

        public TakiGameFactory(IServiceProvider serviceProvider)
        {
            _messageHandler = serviceProvider.GetRequiredService<IMessageHandler>();
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
