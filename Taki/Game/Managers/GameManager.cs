using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taki.Game.Algorithm;
using Taki.Game.Cards;
using Taki.Game.Deck;
using Taki.Game.GameRules;
using Taki.Game.General;
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
        private const int NUMBER_OF_TOTAL_WINNERS = 2;
        private readonly List<IPlayerAlgorithm> algorithms =
        [
            new PlayerAlgorithm(),
            new PlayerHateTakiAlgo()
        ];
        private readonly CardDeck cardDeck;
        private readonly RuleHandler ruleHandler;

        public GameManager(int numberOfPlayers, int numberOfPlayerCards)
        {
            LinkedList<Player> players = new ();
            cardDeck = CardDeckFactory.GenerateCardDeck();
            CreatePlayers(players, numberOfPlayers);
            DealCards(players, numberOfPlayerCards);
            cardDeck.DrawFirstCard();
            ruleHandler = new (players, cardDeck);
        }

        public void StartGame()
        {
            int[] winnerIds = new int[NUMBER_OF_TOTAL_WINNERS];
            for (int i = 0; i < winnerIds.Length; i++)
                winnerIds[i] = GetWinnerById();
            PrintWinnersList(winnerIds);
        }

        //TODO: check problem with getting more than 1 winner.
        private int GetWinnerById()
        {
            while (!ruleHandler.PlayerFinishedHand())
            {
                Console.WriteLine("------------------------");
                ruleHandler.PlayTurn();
                ruleHandler.RequestNextPlayer();
            }
            return ruleHandler.RemoveWinner();
        }

        private void CreatePlayers(LinkedList<Player> players, int numberOfPlayers)
        {
            Random random = new();
            players.AddFirst(new Player(0, new ManualPlayerAlgorithm()));
            for (int i = players.Count; i < numberOfPlayers; i++)
            {
                int index = random.Next(algorithms.Count);
                players.AddLast(new Player(i, algorithms.ElementAt(index)));
                //players.AddLast(new Player(i, new ManualPlayerAlgorithm()));
                Debug.WriteLine(players.ElementAt(i));
            }
        }

        private void DealCards(LinkedList<Player> players, int numberOfPlayerCards)
        {
            for (int j = 0; j < numberOfPlayerCards; j++)
                foreach (Player player in players)
                    player.AddCard(cardDeck.DrawCard());
        }

        private static void PrintWinnersList(int[] winnerIds)
        {
            Console.WriteLine("The winners by order:");
            for (int i = 0; i < winnerIds.Length; i++)
                Console.WriteLine($"{i + 1}. id {winnerIds[i]}");
        }
    }
}
