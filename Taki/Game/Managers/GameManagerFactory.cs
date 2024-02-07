using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taki.Game.Deck;
using Taki.Game.General;

namespace Taki.Game.Managers
{
    enum GameTypeEnum
    {
        Normal,
        Pyramid
    }

    internal class GameManagerFactory
    {
        protected static Communicator communicator = Communicator.GetCommunicator();
        private static readonly int MIN_NUMBER_OF_PLAYERS = 2;
        private static readonly int MAX_NUMBER_OF_PLAYERS = 8;
        private static readonly int MIN_NUMBER_OF_PLAYER_CARDS = 7;
        private static readonly int MAX_NUMBER_OF_PLAYER_CARDS = 20;

        public static GameTypeEnum GetGameType()
        {
            return Utilities.GetEnumFromUser<GameTypeEnum>("of game", GameTypeEnum.Normal.GetHashCode());
        }

        public GameManager CreateNormal()
        {
            int numberOfPlayers = GetNumberOfPlayers(out List<string> names);
            int numberOfPlayerCards = GetNumberOfPlayerCards(numberOfPlayers);

            WriteMessageToScreen(numberOfPlayers, numberOfPlayerCards);

            return new(numberOfPlayers, numberOfPlayerCards, names);
        }

        public GameManager CreatePyramid() 
        {
            int numberOfPlayers = GetNumberOfPlayers(out List<string> names);
            return new PyramidGameManager(numberOfPlayers, names);
        }

        private static int GetNumberOfPlayerCards(int numberOfPlayers)
        {
            int maxNumberOfPlayerCards = CardDeckFactory.MaxNumberOfCards() / numberOfPlayers - 1;
            if (maxNumberOfPlayerCards > MAX_NUMBER_OF_PLAYER_CARDS)
                maxNumberOfPlayerCards = MAX_NUMBER_OF_PLAYER_CARDS;
            Communicator.PrintMessage($"Please enter number of player cards," +
                $" a number between {MIN_NUMBER_OF_PLAYER_CARDS} and {maxNumberOfPlayerCards}");
            int numberOfPlayerCards = GetNumberFromUser();

            if (numberOfPlayerCards > maxNumberOfPlayerCards)
            {
                Communicator.PrintMessage($"Too many cards per player, max is {maxNumberOfPlayerCards}." +
                    $" setting as the max value");
                numberOfPlayerCards = maxNumberOfPlayerCards;

                return numberOfPlayerCards;
            }

            else if (numberOfPlayerCards < MIN_NUMBER_OF_PLAYER_CARDS)
            {
                Communicator.PrintMessage($"Not enough cards per player, " +
                    $"min is {MIN_NUMBER_OF_PLAYER_CARDS} setting as min value");
                numberOfPlayerCards = MIN_NUMBER_OF_PLAYER_CARDS;
            }

            return numberOfPlayerCards;
        }

        private static int GetNumberOfPlayers(out List<string> names)
        {
            Communicator.PrintMessage($"Please enter number of players," +
                $" a number between {MIN_NUMBER_OF_PLAYERS} and {MAX_NUMBER_OF_PLAYERS}");
            int numberOfPlayers = GetNumberFromUser();

            if (numberOfPlayers < MIN_NUMBER_OF_PLAYERS)
            {
                Communicator.PrintMessage($"Not enough players, setting as min value {MIN_NUMBER_OF_PLAYERS}");

                numberOfPlayers = MIN_NUMBER_OF_PLAYERS;
            }
            else if (numberOfPlayers > MAX_NUMBER_OF_PLAYERS)
            {
                Communicator.PrintMessage($"Too many players for the game, setting as max value {MAX_NUMBER_OF_PLAYERS}");

                numberOfPlayers = MAX_NUMBER_OF_PLAYERS;
            }

            names = Enumerable.Range(0, numberOfPlayers).ToList().Select(i =>
            {
                Communicator.PrintMessage($"Please enter name #{i + 1}");
                string? name = Communicator.ReadMessage();

                while (name is null)
                {
                    Communicator.PrintMessage("Please enter a valid name");
                    name = Communicator.ReadMessage();
                }

                return name;
            }).ToList();

            return numberOfPlayers;
        }

        private static int GetNumberFromUser()
        {
            int number;

            while (!int.TryParse(Communicator.ReadMessage(), out number))
                Communicator.PrintMessage("Please enter a number");
            return number;
        }

        private static void WriteMessageToScreen(int numberOfPlayers, int numberOfPlayerCards)
        {
            Communicator.PrintMessage($"{numberOfPlayers} players, {numberOfPlayerCards} cards per player");
            Communicator.PrintMessage();
        }
    }
}
