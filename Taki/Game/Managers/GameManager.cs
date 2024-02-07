using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taki.Game.Algorithm;
using Taki.Game.Deck;
using Taki.Game.GameRules;
using Taki.Game.General;
using Taki.Game.Players;

namespace Taki.Game.Managers
{
    internal class GameManager
    {
        protected Communicator communicator = Communicator.GetCommunicator();
        private static readonly bool FULLY_MANUAL_GAME = true;
        private const int NUMBER_OF_TOTAL_WINNERS = 2;
        private static readonly List<IPlayerAlgorithm> algorithms =
        [
            new PlayerAlgorithm(),
            new PlayerHateTakiAlgo()
        ];
        protected CardDeck cardDeck;
        protected RuleHandler ruleHandler;

        public GameManager(int numberOfPlayers, int numberOfPlayerCards, List<string> names)
        {
            LinkedList<Player> players = new ();
            CardDeckFactory factory = new ();
            cardDeck = factory.NewCardDeck();

            CreatePlayers(players, numberOfPlayers, names);
            DealCards(players, numberOfPlayerCards);
            
            ruleHandler = new(new PlayerHandler(players), cardDeck);

            Initialize(players);
        }

        public void StartGame()
        {
            List<Player> winners = [];
            for (int i = 0; i < NUMBER_OF_TOTAL_WINNERS; i++)
            {
                winners.Add(ruleHandler.GetWinner());
                communicator.PrintMessage($"Winner #{i + 1} is {winners.ElementAt(i).Name}");
                if (i < NUMBER_OF_TOTAL_WINNERS - 1)
                {
                    communicator.PrintMessage("Press any key to continue");
                    communicator.ReadMessage();
                }
            }
            communicator.PrintMessage("The winners by order:");
            winners.ForEach(p => communicator.PrintMessage($"{winners.IndexOf(p)}. {p.Name}"));
        }
        
        private void DealCards(LinkedList<Player> players, int numberOfPlayerCards)
        {
            for (int j = 0; j < numberOfPlayerCards; j++)
                foreach (Player player in players)
                    player.AddCard(cardDeck.DrawCard());
        }

        private static void CreatePlayers(LinkedList<Player> players, int numberOfPlayers, List<string> names)
        {
            Random random = new();
            //players.AddFirst(new Player(new ManualPlayerAlgorithm()));
            //Debug.WriteLine(players.ElementAt(0));

            for (int i = players.Count; i < numberOfPlayers; i++)
            {
                int index = random.Next(algorithms.Count);
                if(FULLY_MANUAL_GAME)
                    players.AddLast(new Player(names.ElementAt(i), new ManualPlayerAlgorithm()));
                else
                    players.AddLast(new Player(names.ElementAt(i), algorithms.ElementAt(index)));
                Debug.WriteLine(players.ElementAt(i));
            }
        }

        protected virtual void Initialize(LinkedList<Player> players)
        {
            cardDeck.DrawFirstCard();
        }

    }
}
