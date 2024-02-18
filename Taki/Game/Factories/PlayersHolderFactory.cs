using Microsoft.Extensions.DependencyInjection;
using Taki.Game.Algorithm;
using Taki.Game.GameRunner;
using Taki.Game.Messages;
using Taki.Game.Players;

namespace Taki.Game.Factories
{
    internal class PlayersHolderFactory
    {
        private readonly ProgramVariables _programVariables;

        public PlayersHolderFactory(ProgramVariables programVariables)
        {
            _programVariables = programVariables;
        }

        private List<Player> GeneratePlayers(IServiceProvider serviceProvider)
        {
            var userCommunicator = serviceProvider.GetRequiredService<IUserCommunicator>();
            var playerAlgorithms = serviceProvider.GetRequiredService<List<IPlayerAlgorithm>>();
            var manualPlayerAlgorithm = serviceProvider.GetRequiredService<ManualPlayerAlgorithm>();
            var random = serviceProvider.GetRequiredService<Random>();
            var gameScore = serviceProvider.GetRequiredService<IGameScore>();

            int numberOfPlayers = GetNumberOfPlayers(userCommunicator);
            int numberOfManualPlayers = GetNumberOfManualPlayer(numberOfPlayers, userCommunicator);

            List<Player> players = Enumerable
                .Range(0, numberOfPlayers)
                .Select(i =>
                {
                    string name = GetNameFromUser(i, userCommunicator);

                    if (numberOfManualPlayers-- > 0)
                    {
                        Player player = new Player(name, manualPlayerAlgorithm, userCommunicator);
                        if(gameScore.DoesUserExist(name))
                        {
                            int score = gameScore.GetScoreByName(name);
                            string? answer = userCommunicator.GetMessageFromUser($"would you like to load the existing " +
                                $"score of {score} for user {name}, y or else to avoid");
                            if(answer != null)
                                player.Score = score;
                        }
                        return player;
                    }

                    int algoRandomIndex = random.Next(playerAlgorithms.Count);

                    return new Player(name, playerAlgorithms[algoRandomIndex], userCommunicator);
                }).ToList();

            userCommunicator.SendMessageToUser("users created are:");
            var playersInormation = players.Select(p => p.GetInformation())
                .ToList();
            userCommunicator.SendMessageToUser(string.Join("\n", playersInormation));
            userCommunicator.SendMessageToUser();

            return players;
        }

        public PlayersHolder GeneratePlayersHandler(IServiceProvider serviceProvider, int maxNumberOfCards)
        {
            var userCommunicator = serviceProvider.GetRequiredService<IUserCommunicator>();
            List<Player> players = GeneratePlayers(serviceProvider);
            int numberOfPlayerCards = GetNumberOfPlayerCards(players.Count, maxNumberOfCards, userCommunicator);

            return new PlayersHolder(players, numberOfPlayerCards, serviceProvider);
        }

        public PlayersHolder GeneratePyramidPlayersHandler(IServiceProvider serviceProvider)
        {
            var userCommunicator = serviceProvider.GetRequiredService<IUserCommunicator>();

            List<Player> pyramidPlayers = GeneratePlayers(serviceProvider)
                .Select(player => 
                (Player)new PyramidPlayer(player, _programVariables.NUMBER_OF_PYRAMID_PLAYER_CARDS)).ToList();

            return new PyramidPlayersHolder(pyramidPlayers, _programVariables.NUMBER_OF_PYRAMID_PLAYER_CARDS, serviceProvider);
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

        private int GetNumberOfPlayerCards(int numberOfPlayers, int maxNumberOfCards, IUserCommunicator userCommunicator)
        {
            int maxNumberOfPlayerCards = maxNumberOfCards / numberOfPlayers - 1;

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
