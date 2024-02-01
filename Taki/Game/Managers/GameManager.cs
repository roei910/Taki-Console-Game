﻿using System;
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
    //TODO: fix bug where the game gets stuck for no reason waiting for the computer to play a hand
    enum GameTypeEnum
    {
        Normal,
        Pyramid
    }

    internal class GameManager
    {
        private const int NUMBER_OF_TOTAL_WINNERS = 2;
        private static readonly List<IPlayerAlgorithm> algorithms =
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
            for (int i = players.Count; i < numberOfPlayers; i++)
            {
                int index = random.Next(algorithms.Count);
                players.AddLast(new Player(algorithms.ElementAt(index)));
                //players.AddLast(new Player(i, new ManualPlayerAlgorithm()));
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
