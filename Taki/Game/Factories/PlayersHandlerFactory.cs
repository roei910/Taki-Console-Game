using Microsoft.Extensions.DependencyInjection;
using Taki.Game.GameRules;
using Taki.Game.Handlers;
using Taki.Game.Interfaces;
using Taki.Game.Players;

namespace Taki.Game.Factories
{
    internal class PlayersHandlerFactory
    {
        private readonly IMessageHandler _messageHandler;
        private readonly List<IPlayerAlgorithm> _playerAlgorithms;

        //TODO: remove from here
        private static readonly int MIN_NUMBER_OF_PLAYERS = 2;
        private static readonly int MAX_NUMBER_OF_PLAYERS = 8;
        //TODO: move all consts from code
        private const int MIN_NUMBER_OF_PLAYER_CARDS = 7;
        private const int MAX_NUMBER_OF_PLAYER_CARDS = 20;
        private const int NUMBER_OF_PYRAMID_PLAYER_CARDS = 10;

        public PlayersHandlerFactory(IServiceProvider serviceProvider, 
            List<IPlayerAlgorithm> playerAlgorithms)
        {
            _messageHandler = serviceProvider.GetRequiredService<IMessageHandler>();
            _playerAlgorithms = playerAlgorithms;
        }

        private List<Player> GeneratePlayers(int numberOfPlayers)
        {
            Random random = new ();
            List<Player> players = Enumerable
                .Range(0, numberOfPlayers)
                .Select(i =>
                {
                    string name = GetNameFromUser(i);
                    int algoRandomIndex = random.Next(_playerAlgorithms.Count);
                    return new Player(name, _playerAlgorithms.ElementAt(algoRandomIndex));
                }).ToList();

            _messageHandler.SendMessageToUser("users created are:");
            var playersInormation = players.Select(p => p.GetInformation())
                .ToList();
            _messageHandler.SendMessageToUser(string.Join("\n", playersInormation));
            return players;
        }

        public PlayersHandler GeneratePlayersHandler()
        {
            int numberOfPlayers = GetNumberOfPlayers();
            int numberOfPlayerCards = GetNumberOfPlayerCards(numberOfPlayers);
            List<Player> players = GeneratePlayers(numberOfPlayers);

            return new PlayersHandler(players, numberOfPlayerCards);
        }

        public PlayersHandler GeneratePyramidPlayersHandler()
        {
            int numberOfPlayers = GetNumberOfPlayers();

            List<Player> pyramidPlayers = GeneratePlayers(numberOfPlayers)
                .Select(player => 
                (Player)new PyramidPlayer(player, NUMBER_OF_PYRAMID_PLAYER_CARDS)).ToList();

            return new PyramidPlayersHandler(pyramidPlayers, NUMBER_OF_PYRAMID_PLAYER_CARDS);
        }

        private int GetNumberOfPlayers()
        {
            _messageHandler.SendMessageToUser($"Please enter number of players," +
                $" a number between {MIN_NUMBER_OF_PLAYERS} and {MAX_NUMBER_OF_PLAYERS}");

            int numberOfPlayers = _messageHandler.GetNumberFromUser();

            if (numberOfPlayers < MIN_NUMBER_OF_PLAYERS)
            {
                _messageHandler.SendMessageToUser($"Not enough players, setting as min value {MIN_NUMBER_OF_PLAYERS}");

                numberOfPlayers = MIN_NUMBER_OF_PLAYERS;
            }
            else if (numberOfPlayers > MAX_NUMBER_OF_PLAYERS)
            {
                _messageHandler.SendMessageToUser($"Too many players for the game, setting as max value {MAX_NUMBER_OF_PLAYERS}");

                numberOfPlayers = MAX_NUMBER_OF_PLAYERS;
            }

            return numberOfPlayers;
        }

        private string GetNameFromUser(int index)
        {
            _messageHandler.SendMessageToUser($"Please enter a name #{index + 1}");
            string? name = _messageHandler.GetMessageFromUser();
            while(name == null)
            {
                _messageHandler.SendMessageToUser($"Please enter a valid name #{index + 1}");
                name = _messageHandler.GetMessageFromUser();
            }
            return name.Split(" ").ElementAt(0);
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

        
    }
}
