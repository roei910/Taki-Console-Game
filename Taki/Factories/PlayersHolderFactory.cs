﻿using Taki.Models.Algorithm;
using Taki.Models.GameLogic;
using Taki.Models.Players;
using Taki.Shared.Interfaces;
using Taki.Shared.Models.Dto;

namespace Taki.Factories
{
    internal class PlayersHolderFactory
    {
        private readonly ProgramVariables _programVariables;
        private readonly IUserCommunicator _userCommunicator;
        private readonly List<IPlayerAlgorithm> _playerAlgorithms;
        private readonly Random _random;
        private readonly IGameScore _gameScore;
        private readonly IManualPlayerAlgorithm _manualPlayerAlgorithm;
        private readonly IDal<PlayerDto> _playersDatabase;

        public PlayersHolderFactory(ProgramVariables programVariables, IUserCommunicator userCommunicator,
            List<IPlayerAlgorithm> playerAlgorithms, Random random, IGameScore gameScore,
            IManualPlayerAlgorithm manualPlayerAlgorithm, IDal<PlayerDto> playerDatabase)
        {
            _programVariables = programVariables;
            _userCommunicator = userCommunicator;
            _playerAlgorithms = playerAlgorithms;
            _random = random;
            _gameScore = gameScore;
            _manualPlayerAlgorithm = manualPlayerAlgorithm;
            _playersDatabase = playerDatabase;
        }

        public PlayersHolder GeneratePlayersHandler(int maxNumberOfCards)
        {
            List<Player> players = GeneratePlayers();
            int numberOfPlayerCards = GetNumberOfPlayerCards(players.Count, maxNumberOfCards);

            return new PlayersHolder(players, numberOfPlayerCards, _userCommunicator, _playersDatabase);
        }

        public PlayersHolder GeneratePyramidPlayersHandler()
        {
            List<Player> pyramidPlayers = GeneratePlayers()
                .Select(player =>
                (Player)new PyramidPlayer(player, _programVariables.NUMBER_OF_PYRAMID_PLAYER_CARDS)).ToList();

            return new PyramidPlayersHolder(pyramidPlayers, _programVariables.NUMBER_OF_PYRAMID_PLAYER_CARDS,
                _userCommunicator, _playersDatabase);
        }

        private List<Player> GeneratePlayers()
        {
            int numberOfPlayers = GetNumberOfPlayers();
            int numberOfManualPlayers = GetNumberOfManualPlayer(numberOfPlayers);
            int algoPlayers = 0;

            List<Player> players = Enumerable
                .Range(0, numberOfPlayers)
                .Select(i =>
                {
                    if (numberOfManualPlayers-- > 0)
                    {
                        string name = GetNameFromUser(i);

                        Player player = new Player(name, _manualPlayerAlgorithm, _userCommunicator);
                        int score = _gameScore.GetScoreByName(name);

                        if (score != GameScore.NO_SCORE)
                        {
                            string? answer = _userCommunicator.GetMessageFromUser($"would you like to load the existing " +
                                $"score of {score} for user {name}, y or else to avoid");
                            if (answer == "y")
                                player.Score = score;
                        }

                        return player;
                    }

                    int algoRandomIndex = _random.Next(_playerAlgorithms.Count);

                    return new Player($"Player {++algoPlayers}", _playerAlgorithms[algoRandomIndex], _userCommunicator);
                }).ToList();

            _userCommunicator.SendMessageToUser("users created are:");
            var playersInormation = players.Select((p, i) => $"{i + 1}. {p.GetInformation()}")
                .ToList();
            _userCommunicator.SendMessageToUser(string.Join("\n", playersInormation));
            _userCommunicator.SendMessageToUser();

            return players;
        }

        private int GetNumberOfPlayers()
        {
            string message = $"Please enter number of players," +
                $" a number between {_programVariables.MIN_NUMBER_OF_PLAYERS} and {_programVariables.MAX_NUMBER_OF_PLAYERS}";
            int numberOfPlayers = _userCommunicator.GetNumberFromUser(message);

            if (numberOfPlayers < _programVariables.MIN_NUMBER_OF_PLAYERS)
            {
                _userCommunicator.SendMessageToUser($"Not enough players, setting as min value {_programVariables.MIN_NUMBER_OF_PLAYERS}");

                numberOfPlayers = _programVariables.MIN_NUMBER_OF_PLAYERS;
            }
            else if (numberOfPlayers > _programVariables.MAX_NUMBER_OF_PLAYERS)
            {
                _userCommunicator.SendMessageToUser($"Too many players for the game, setting as max value {_programVariables.MAX_NUMBER_OF_PLAYERS}");

                numberOfPlayers = _programVariables.MAX_NUMBER_OF_PLAYERS;
            }

            return numberOfPlayers;
        }

        private int GetNumberOfManualPlayer(int maxPlayers)
        {
            int numberOfPlayers = _userCommunicator.GetNumberFromUser(
                $"Please enter number of manual players or 0 for none");

            if (numberOfPlayers < 0)
                return 0;

            if (numberOfPlayers > maxPlayers)
                return maxPlayers;

            return numberOfPlayers;
        }

        private string GetNameFromUser(int index)
        {
            string? name = _userCommunicator.GetMessageFromUser(
                $"Please enter a name #{index + 1}");

            while (name is null)
                name = _userCommunicator.GetMessageFromUser(
                    $"Please enter a valid name #{index + 1}");

            return name.Split(" ").ElementAt(0);
        }

        private int GetNumberOfPlayerCards(int numberOfPlayers, int maxNumberOfCards)
        {
            int maxNumberOfPlayerCards = maxNumberOfCards / numberOfPlayers - 1;

            if (maxNumberOfPlayerCards > _programVariables.MAX_NUMBER_OF_PLAYER_CARDS)
                maxNumberOfPlayerCards = _programVariables.MAX_NUMBER_OF_PLAYER_CARDS;

            int numberOfPlayerCards = _userCommunicator.GetNumberFromUser(
                "please enter a number of player cards");

            if (numberOfPlayerCards > maxNumberOfPlayerCards)
            {
                _userCommunicator.SendMessageToUser($"Too many cards per player, max is {maxNumberOfPlayerCards}." +
                    $" setting as the max value");

                return maxNumberOfPlayerCards;
            }

            if (numberOfPlayerCards < _programVariables.MIN_NUMBER_OF_PLAYER_CARDS)
            {
                _userCommunicator.SendMessageToUser($"Not enough cards per player, " +
                    $"min is {_programVariables.MIN_NUMBER_OF_PLAYER_CARDS} setting as min value");

                return _programVariables.MIN_NUMBER_OF_PLAYER_CARDS;
            }

            return numberOfPlayerCards;
        }
    }
}
