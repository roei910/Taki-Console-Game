using Microsoft.Extensions.DependencyInjection;
using Taki.Game.Communicators;
using Taki.Game.GameRules;
using Taki.Game.General;
using Taki.Game.Handlers;
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
        //TODO: move from here
        private static readonly int MIN_NUMBER_OF_PLAYER_CARDS = 7;
        private static readonly int MAX_NUMBER_OF_PLAYER_CARDS = 20;

        private readonly IMessageHandler _messageHandler;
        private readonly IServiceProvider _serviceProvider;
        private readonly Utilities _utilities;

        public TakiGameFactory(IServiceProvider serviceProvider)
        {
            _messageHandler = serviceProvider.GetRequiredService<IMessageHandler>();
            _serviceProvider = serviceProvider;
            _utilities = serviceProvider.GetRequiredService<Utilities>();
        }

        public TakiGameRunner ChooseTypeOfGame(GameHandlers gameHandlers)
        {
            GameTypeEnum typeOfGame = _utilities
                .GetEnumFromUser<GameTypeEnum>("of game", GameTypeEnum.Normal.GetHashCode());

            switch (typeOfGame)
            {
                case GameTypeEnum.Normal:
                    int numberOfPlayerCards = GetNumberOfPlayerCards(gameHandlers.GetPlayersHandler().GetNumberOfPlayers());
                    return new TakiGameRunner(gameHandlers, _serviceProvider, numberOfPlayerCards);

                //case GameTypeEnum.Pyramid:
                    //PyramidRuleHandler pyramidRuleHandler = new(gameHandlers);
                    //return new(pyramidRuleHandler);
                    //break;
                default:
                    throw new Exception("type enum was wrong");
            }
        }

        private int GetNumberOfPlayerCards(int numberOfPlayers)
        {
            int maxNumberOfPlayerCards = CardsHandlerFactory.MaxNumberOfCards() / numberOfPlayers - 1;

            if (maxNumberOfPlayerCards > MAX_NUMBER_OF_PLAYER_CARDS)
                maxNumberOfPlayerCards = MAX_NUMBER_OF_PLAYER_CARDS;

            _messageHandler.SendMessageToUser("please enter a number of player cards");
            int numberOfPlayerCards = _messageHandler.GetNumberFromUser();

            if (numberOfPlayerCards > maxNumberOfPlayerCards)
            {
                _messageHandler.SendMessageToUser($"Too many cards per player, max is {maxNumberOfPlayerCards}." +
                    $" setting as the max value");

                return maxNumberOfPlayerCards;
            }

            if (numberOfPlayerCards < MIN_NUMBER_OF_PLAYER_CARDS)
            {
                _messageHandler.SendMessageToUser($"Not enough cards per player, " +
                    $"min is {MIN_NUMBER_OF_PLAYER_CARDS} setting as min value");

                return MIN_NUMBER_OF_PLAYER_CARDS;
            }

            return numberOfPlayerCards;
        }

        


        private void WriteMessageToScreen(int numberOfPlayers, int numberOfPlayerCards)
        {
            _messageHandler.SendMessageToUser($"{numberOfPlayers} players, {numberOfPlayerCards} cards per player");
            _messageHandler.SendMessageToUser();
        }
    }
}
