using Microsoft.Extensions.DependencyInjection;
using Taki.Game.Algorithm;
using Taki.Game.GameRules;
using Taki.Game.Handlers;
using Taki.Game.Messages;
using Taki.Game.Players;

namespace Taki.Game.Factories
{
    internal class PlayersHandlerFactory
    {
        private readonly ProgramVariables _programVariables;

        public PlayersHandlerFactory(ProgramVariables programVariables)
        {
            _programVariables = programVariables;
        }

        private List<Player> GeneratePlayers(int numberOfPlayers, IServiceProvider serviceProvider)
        {
            var userCommunicator = serviceProvider.GetRequiredService<IUserCommunicator>();
            var playerAlgorithms = serviceProvider.GetRequiredService<List<IPlayerAlgorithm>>();
            var manualPlayerAlgorithm = serviceProvider.GetRequiredService<ManualPlayerAlgorithm>();
            var random = serviceProvider.GetRequiredService<Random>();
            int numberOfManualPlayers = GetNumberOfManualPlayer(numberOfPlayers, userCommunicator);

            List<Player> players = Enumerable
                .Range(0, numberOfPlayers)
                .Select(i =>
                {
                    string name = GetNameFromUser(i, userCommunicator);

                    if (numberOfManualPlayers-- > 0)
                        return new Player(name, manualPlayerAlgorithm);

                    int algoRandomIndex = random.Next(playerAlgorithms.Count);

                    return new Player(name, playerAlgorithms[algoRandomIndex]);
                }).ToList();

            userCommunicator.SendMessageToUser("users created are:");
            var playersInormation = players.Select(p => p.GetInformation())
                .ToList();
            userCommunicator.SendMessageToUser(string.Join("\n", playersInormation));

            return players;
        }

        public PlayersHandler GeneratePlayersHandler(IServiceProvider serviceProvider, int maxCards)
        {
            //TODO: combine with the pyramid somehow
            var userCommunicator = serviceProvider.GetRequiredService<IUserCommunicator>();
            int numberOfPlayers = GetNumberOfPlayers(userCommunicator);
            int numberOfPlayerCards = GetNumberOfPlayerCards(numberOfPlayers, maxCards, userCommunicator);
            List<Player> players = GeneratePlayers(numberOfPlayers, serviceProvider);

            return new PlayersHandler(players, numberOfPlayerCards);
        }

        public PlayersHandler GeneratePyramidPlayersHandler(IServiceProvider serviceProvider)
        {
            var userCommunicator = serviceProvider.GetRequiredService<IUserCommunicator>();
            var playerAlgorithms = serviceProvider.GetRequiredService<List<IPlayerAlgorithm>>();
            var manualPlayerAlgorithm = serviceProvider.GetRequiredService<ManualPlayerAlgorithm>();
            int numberOfPlayers = GetNumberOfPlayers(userCommunicator);

            List<Player> pyramidPlayers = GeneratePlayers(numberOfPlayers, serviceProvider)
                .Select(player => 
                (Player)new PyramidPlayer(player, _programVariables.NUMBER_OF_PYRAMID_PLAYER_CARDS)).ToList();

            return new PyramidPlayersHandler(pyramidPlayers, _programVariables.NUMBER_OF_PYRAMID_PLAYER_CARDS);
        }

        private int GetNumberOfPlayers(IUserCommunicator userCommunicator)
        {
            string message = $"Please enter number of players," +
                $" a number between {_programVariables.MIN_NUMBER_OF_PLAYERS} and {_programVariables.MAX_NUMBER_OF_PLAYERS}";
            int numberOfPlayers = userCommunicator.GetNumberFromUser(message);

            if (numberOfPlayers < _programVariables.MIN_NUMBER_OF_PLAYERS)
            {
                userCommunicator.SendMessageToUser($"Not enough players, setting as min value {_programVariables.MIN_NUMBER_OF_PLAYERS}");

                numberOfPlayers = _programVariables.MIN_NUMBER_OF_PLAYERS;
            }
            else if (numberOfPlayers > _programVariables.MAX_NUMBER_OF_PLAYERS)
            {
                userCommunicator.SendMessageToUser($"Too many players for the game, setting as max value {_programVariables.MAX_NUMBER_OF_PLAYERS}");

                numberOfPlayers = _programVariables.MAX_NUMBER_OF_PLAYERS;
            }

            return numberOfPlayers;
        }

        private int GetNumberOfManualPlayer(int maxPlayers, IUserCommunicator userCommunicator)
        {
            int numberOfPlayers = userCommunicator.GetNumberFromUser(
                $"Please enter number of manual players or 0 for none");

            if (numberOfPlayers < 0)
                return 0;

            if (numberOfPlayers > maxPlayers)
                return maxPlayers;

            return numberOfPlayers;
        }

        private string GetNameFromUser(int index, IUserCommunicator userCommunicator)
        {
            string? name = userCommunicator.GetMessageFromUser(
                $"Please enter a name #{index + 1}");

            while(name == null)
                name = userCommunicator.GetMessageFromUser(
                    $"Please enter a valid name #{index + 1}");

            return name.Split(" ").ElementAt(0);
        }

        private int GetNumberOfPlayerCards(int numberOfPlayers, int maxCards, IUserCommunicator userCommunicator)
        {
            int maxNumberOfPlayerCards = maxCards / numberOfPlayers - 1;

            if (maxNumberOfPlayerCards > _programVariables.MAX_NUMBER_OF_PLAYER_CARDS)
                maxNumberOfPlayerCards = _programVariables.MAX_NUMBER_OF_PLAYER_CARDS;

            int numberOfPlayerCards = userCommunicator.GetNumberFromUser(
                "please enter a number of player cards");

            if (numberOfPlayerCards > maxNumberOfPlayerCards)
            {
                userCommunicator.SendMessageToUser($"Too many cards per player, max is {maxNumberOfPlayerCards}." +
                    $" setting as the max value");

                return maxNumberOfPlayerCards;
            }

            if (numberOfPlayerCards < _programVariables.MIN_NUMBER_OF_PLAYER_CARDS)
            {
                userCommunicator.SendMessageToUser($"Not enough cards per player, " +
                    $"min is {_programVariables.MIN_NUMBER_OF_PLAYER_CARDS} setting as min value");

                return _programVariables.MIN_NUMBER_OF_PLAYER_CARDS;
            }

            return numberOfPlayerCards;
        }
    }
}
