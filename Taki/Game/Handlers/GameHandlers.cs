using Microsoft.Extensions.DependencyInjection;
using Taki.Game.GameRules;
using Taki.Game.Interfaces;

namespace Taki.Game.Handlers
{
    internal class GameHandlers
    {
        private readonly PlayersHandler _playerHandler;
        private readonly CardsHandler _cardsHandler;
        private readonly IServiceProvider _serviceProvider;

        public GameHandlers(
            PlayersHandler playerHandler,
            CardsHandler cardsHandler,
            IServiceProvider serviceProvider)
        {
            _playerHandler = playerHandler;
            _cardsHandler = cardsHandler;
            _serviceProvider = serviceProvider;
        }

        public PlayersHandler GetPlayersHandler()
        {
            return _playerHandler;
        }

        public CardsHandler GetCardsHandler()
        {
            return _cardsHandler;
        }

        public IMessageHandler GetMessageHandler()
        {
            return _serviceProvider.GetRequiredService<IMessageHandler>();
        }

        public IServiceProvider GetServiceProvider()
        {
            return _serviceProvider;
        }
    }
}
