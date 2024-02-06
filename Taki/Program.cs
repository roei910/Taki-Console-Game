using System.Drawing;
using Taki.Game.Cards;
using Taki.Game.Deck;
using Taki.Game.General;
using Taki.Game.Managers;

internal class Program
{
    private static readonly int MIN_NUMBER_OF_PLAYERS = 2;
    private static readonly int MAX_NUMBER_OF_PLAYERS = 8;
    private static readonly int MIN_NUMBER_OF_PLAYER_CARDS = 7;
    private static readonly int MAX_NUMBER_OF_PLAYER_CARDS = 20;

    private static void Main(string[] args)
    {
        GameManager gameManager;
        GameTypeEnum typeOfGame = Utilities.GetEnumFromUser<GameTypeEnum>("of game", GameTypeEnum.Normal.GetHashCode());
        List<string> names = [];
        int numberOfPlayers, numberOfPlayerCards;
        numberOfPlayers = GetNumberOfPlayers(names);
        switch (typeOfGame)
        {
            case GameTypeEnum.Normal:
                numberOfPlayerCards = GetNumberOfPlayerCards(numberOfPlayers);
                gameManager = new (numberOfPlayers, numberOfPlayerCards, names);
                Console.WriteLine("Starting a new game of TAKI!");
                WriteMessageToScreen(numberOfPlayers, numberOfPlayerCards);
                gameManager.StartGame();
                break;
            case GameTypeEnum.Pyramid:
                gameManager = new PyramidGameManager(numberOfPlayers, names);
                Console.WriteLine("Starting a new game of TAKI pyramid edition!");
                gameManager.StartGame();
                break;
            default:
                break;
        }
    }

    private static int GetNumberOfPlayerCards(int numberOfPlayers)
    {
        int maxNumberOfPlayerCards = CardDeckFactory.MaxNumberOfCards() / numberOfPlayers - 1;
        if (maxNumberOfPlayerCards > MAX_NUMBER_OF_PLAYER_CARDS)
            maxNumberOfPlayerCards = MAX_NUMBER_OF_PLAYER_CARDS;
        Console.WriteLine($"Please enter number of player cards," +
            $" a number between {MIN_NUMBER_OF_PLAYER_CARDS} and {maxNumberOfPlayerCards}");
        int numberOfPlayerCards = GetNumberFromUser();

        if (numberOfPlayerCards > maxNumberOfPlayerCards)
        {
            Console.WriteLine($"Too many cards per player, max is {maxNumberOfPlayerCards}." +
                $" setting as the max value");
            numberOfPlayerCards = maxNumberOfPlayerCards;
        }
        else if (numberOfPlayerCards < MIN_NUMBER_OF_PLAYER_CARDS)
        {
            Console.WriteLine($"Not enough cards per player, " +
                $"min is {MIN_NUMBER_OF_PLAYER_CARDS} setting as min value");
            numberOfPlayerCards = MIN_NUMBER_OF_PLAYER_CARDS;
        }

        return numberOfPlayerCards;
    }

    private static int GetNumberOfPlayers(List<string> names)
    {
        Console.WriteLine($"Please enter number of players," +
            $" a number between {MIN_NUMBER_OF_PLAYERS} and {MAX_NUMBER_OF_PLAYERS}");
        int numberOfPlayers = GetNumberFromUser();

        if (numberOfPlayers < MIN_NUMBER_OF_PLAYERS)
        {
            Console.WriteLine($"Not enough players, setting as min value {MIN_NUMBER_OF_PLAYERS}");
            return MIN_NUMBER_OF_PLAYERS;
        }
        else if(numberOfPlayers > MAX_NUMBER_OF_PLAYERS)
        {
            Console.WriteLine($"Too many players for the game, setting as max value {MAX_NUMBER_OF_PLAYERS}");
            return MAX_NUMBER_OF_PLAYERS;
        }
        Enumerable.Range(0, numberOfPlayers).ToList().ForEach(i =>
        {
            Console.WriteLine($"Please enter name #{i + 1}");
            string? name = Console.ReadLine();
            while (name == null)
            {
                Console.WriteLine("Please enter a valid name");
                name = Console.ReadLine();
            }
            names.Add(name);
        });
        return numberOfPlayers;
    }

    private static int GetNumberFromUser()
    {
        int number;
        while (!int.TryParse(Console.ReadLine(), out number))
            Console.WriteLine("Please enter a number");
        return number;
    }

    private static void WriteMessageToScreen(int numberOfPlayers, int numberOfPlayerCards)
    {
        Console.WriteLine($"{numberOfPlayers} players, {numberOfPlayerCards} cards per player");
        Console.WriteLine();
    }
}