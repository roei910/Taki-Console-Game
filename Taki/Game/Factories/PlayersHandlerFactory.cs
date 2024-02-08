using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taki.Game.Algorithm;
using Taki.Game.Communicators;
using Taki.Game.GameRules;
using Taki.Game.Interfaces;
using Taki.Game.Players;

namespace Taki.Game.Factories
{
    internal class PlayersHandlerFactory
    {
        private readonly IMessageHandler _messageHandler;
        private readonly List<IPlayerAlgorithm> _playerAlgorithms;
        private static readonly int MIN_NUMBER_OF_PLAYERS = 2;
        private static readonly int MAX_NUMBER_OF_PLAYERS = 8;

        public PlayersHandlerFactory(IServiceProvider serviceProvider, 
            List<IPlayerAlgorithm> playerAlgorithms)
        {
            _messageHandler = serviceProvider.GetRequiredService<IMessageHandler>();
            _playerAlgorithms = playerAlgorithms;
        }

        public PlayersHandler GeneratePlayersHandler(int numberOfPlayers)
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

            return new PlayersHandler(players);
        }

        public PlayersHandler GeneratePlayersHandler()
        {
            int numberOfPlayers = GetNumberOfPlayers();
            return GeneratePlayersHandler(numberOfPlayers);
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
    }
}
