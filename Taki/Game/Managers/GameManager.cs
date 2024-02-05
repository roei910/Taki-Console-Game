using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taki.Game.Algorithm;
using Taki.Game.Deck;
using Taki.Game.GameRules;
using Taki.Game.Players;

namespace Taki.Game.Managers
{
    enum GameTypeEnum
    {
        Normal,
        Pyramid
    }

    internal class GameManager
    {
        private static readonly bool FULLY_MANUAL_GAME = false;
        private const int NUMBER_OF_TOTAL_WINNERS = 2;
        private static readonly List<IPlayerAlgorithm> algorithms =
        [
            new PlayerAlgorithm(),
            new PlayerHateTakiAlgo()
        ];
        protected CardDeck cardDeck;
        protected RuleHandler ruleHandler;

        public GameManager(int numberOfPlayers, int numberOfPlayerCards)
        {
            LinkedList<Player> players = new ();
            cardDeck = CardDeckFactory.GenerateCardDeck();
            CreatePlayers(players, numberOfPlayers);
            DealCards(players, numberOfPlayerCards);
            ruleHandler = new(new PlayerHandler(players), cardDeck);
            Initialize(players);
        }

        protected virtual void Initialize(LinkedList<Player> players)
        {
            cardDeck.DrawFirstCard();
        }

        public void StartGame()
        {
            int[] winnerIds = new int[NUMBER_OF_TOTAL_WINNERS];
            for (int i = 0; i < winnerIds.Length; i++)
            {
                winnerIds[i] = ruleHandler.GetWinner();
                Console.WriteLine($"Winner #{i+1} is Player[{winnerIds[i]}]");
                if (i < winnerIds.Length-1)
                {
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey();
                }
            }
            PrintWinnersList(winnerIds);
        }
        
        private void DealCards(LinkedList<Player> players, int numberOfPlayerCards)
        {
            for (int j = 0; j < numberOfPlayerCards; j++)
                foreach (Player player in players)
                    player.AddCard(cardDeck.DrawCard());
        }

        private static void CreatePlayers(LinkedList<Player> players, int numberOfPlayers)
        {
            Random random = new();
            //players.AddFirst(new Player(new ManualPlayerAlgorithm()));
            //Debug.WriteLine(players.ElementAt(0));

            for (int i = players.Count; i < numberOfPlayers; i++)
            {
                int index = random.Next(algorithms.Count);
                if(FULLY_MANUAL_GAME)
                    players.AddLast(new Player(new ManualPlayerAlgorithm()));
                else
                    players.AddLast(new Player(algorithms.ElementAt(index)));
                Debug.WriteLine(players.ElementAt(i));
            }
        }

        private static void PrintWinnersList(int[] winnerIds)
        {
            Console.WriteLine("The winners by order:");
            for (int i = 0; i < winnerIds.Length; i++)
                Console.WriteLine($"{i + 1}. id {winnerIds[i]}");
        }
    }
}
