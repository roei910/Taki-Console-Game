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
    //TODO: game stuck after change-color card
    //TODO: fix messages in screen not appearing in the right timeline
    //TODO: if no one can play the game is a tie, must declare it.
    //TODO: if no cards to deal then we continue without dealing cards
    //TODO: Taki - problem with the last card not registering as unique and executing the functionality  for the card
    //TODO: Taki - problems with stacking and handling of unique cards
    //TODO: +2, stacking +2's - after +2 is done iwant to be able to put another +2 if i can.
    //TODO: check error stuck after switch cards with direction
    //TODO: fix error cannot put change direction on plus same color
    enum GameTypeEnum
    {
        Normal,
        Pyramid
    }

    internal class GameManager
    {
        private static readonly int NUMBER_OF_PLAYER_CARDS_PYRAMID = 10;
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
                winnerIds[i] = ruleHandler.GetWinner();
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
            players.AddFirst(new Player(new ManualPlayerAlgorithm()));
            Debug.WriteLine(players.ElementAt(0));

            for (int i = players.Count; i < numberOfPlayers; i++)
            {
                int index = random.Next(algorithms.Count);
                players.AddLast(new Player(algorithms.ElementAt(index)));
                //players.AddLast(new Player(new ManualPlayerAlgorithm()));
                Debug.WriteLine(players.ElementAt(i));
            }
        }

        private static void CreatePlayersPyramid(LinkedList<Player> players, int numberOfPlayers)
        {
            Random random = new();
            players.AddFirst(new PyramidPlayer(new Player(new ManualPlayerAlgorithm())));
            Debug.WriteLine(players.ElementAt(0));

            for (int i = players.Count; i < numberOfPlayers; i++)
            {
                int index = random.Next(algorithms.Count);
                players.AddLast(new PyramidPlayer(new Player(algorithms.ElementAt(index))));
                //players.AddLast(new Player(i, new ManualPlayerAlgorithm()));
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
