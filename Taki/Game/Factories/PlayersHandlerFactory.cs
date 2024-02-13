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
        private readonly IPlayerAlgorithm _manualPlayerAlgorithm;

        public PlayersHandlerFactory(IServiceProvider serviceProvider, 
            List<IPlayerAlgorithm> playerAlgorithms)
        {
            _messageHandler = serviceProvider.GetRequiredService<IMessageHandler>();
            _playerAlgorithms = playerAlgorithms;
            _manualPlayerAlgorithm = serviceProvider.GetRequiredService<IPlayerAlgorithm>();
        }

        private List<Player> GeneratePlayers(int numberOfPlayers)
        {
            Random random = new ();
            int numberOfManualPlayers = GetNumberOfManualPlayer(numberOfPlayers);

            List<Player> players = Enumerable
                .Range(0, numberOfPlayers)
                .Select(i =>
                {
                    string name = GetNameFromUser(i);

                    if (numberOfManualPlayers-- > 0)
                        return new Player(name, _manualPlayerAlgorithm);

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
                (Player)new PyramidPlayer(player, Constants.NUMBER_OF_PYRAMID_PLAYER_CARDS)).ToList();

            return new PyramidPlayersHandler(pyramidPlayers, Constants.NUMBER_OF_PYRAMID_PLAYER_CARDS);
        }

        private int GetNumberOfPlayers()
        {
            _messageHandler.SendMessageToUser($"Please enter number of players," +
                $" a number between {Constants.MIN_NUMBER_OF_PLAYERS} and {Constants.MAX_NUMBER_OF_PLAYERS}");

            int numberOfPlayers = _messageHandler.GetNumberFromUser();

            if (numberOfPlayers < Constants.MIN_NUMBER_OF_PLAYERS)
            {
                _messageHandler.SendMessageToUser($"Not enough players, setting as min value {Constants.MIN_NUMBER_OF_PLAYERS}");

                numberOfPlayers = Constants.MIN_NUMBER_OF_PLAYERS;
            }
            else if (numberOfPlayers > Constants.MAX_NUMBER_OF_PLAYERS)
            {
                _messageHandler.SendMessageToUser($"Too many players for the game, setting as max value {Constants.MAX_NUMBER_OF_PLAYERS}");

                numberOfPlayers = Constants.MAX_NUMBER_OF_PLAYERS;
            }

            return numberOfPlayers;
        }

        private int GetNumberOfManualPlayer(int maxPlayers)
        {
            _messageHandler.SendMessageToUser($"Please enter number of manual players or 0 for none");
            int numberOfPlayers = _messageHandler.GetNumberFromUser();

            if (numberOfPlayers < 0)
                return 0;

            if (numberOfPlayers > maxPlayers)
                return maxPlayers;

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

            if (maxNumberOfPlayerCards > Constants.MAX_NUMBER_OF_PLAYER_CARDS)
                maxNumberOfPlayerCards = Constants.MAX_NUMBER_OF_PLAYER_CARDS;

            _messageHandler.SendMessageToUser("please enter a number of player cards");
            int numberOfPlayerCards = _messageHandler.GetNumberFromUser();

            if (numberOfPlayerCards > maxNumberOfPlayerCards)
            {
                _messageHandler.SendMessageToUser($"Too many cards per player, max is {maxNumberOfPlayerCards}." +
                    $" setting as the max value");

                return maxNumberOfPlayerCards;
            }

            if (numberOfPlayerCards < Constants.MIN_NUMBER_OF_PLAYER_CARDS)
            {
                _messageHandler.SendMessageToUser($"Not enough cards per player, " +
                    $"min is {Constants.MIN_NUMBER_OF_PLAYER_CARDS} setting as min value");

                return Constants.MIN_NUMBER_OF_PLAYER_CARDS;
            }

            return numberOfPlayerCards;
        }
    }
}
