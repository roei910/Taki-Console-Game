﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taki.Game.Cards;
using Taki.Game.Deck;
using Taki.Game.General;
using Taki.Game.Players;

namespace Taki.Game.GameRules
{
    internal class PlayerHandler(LinkedList<Player> players)
    {
        private readonly LinkedList<Player> players = players;
        public Player CurrentPlayer = players.First();
        public void DrawCards(int numberOfCards, CardDeck cardDeck)
        {
            Enumerable.Range(0, numberOfCards).ToList()
                    .ForEach(x => CurrentPlayer.AddCard(cardDeck.DrawCard()));
            Utilities.PrintConsoleError($"Player[{CurrentPlayer.Id}]: drew {numberOfCards} card(s)");
        }

        public void NextPlayer(bool isDirectionNormal)
        {
            if (isDirectionNormal)
            {
                players.RemoveFirst();
                players.AddLast(CurrentPlayer);
                CurrentPlayer = players.First();
            }
            else
            {
                CurrentPlayer = players.Last();
                players.RemoveLast();
                players.AddFirst(CurrentPlayer);
            }
        }

        public void ReturnUnhandledCard(Card playerCard)
        {
            CurrentPlayer.AddCard(playerCard);
        }

        public Color GetColorFromPlayer()
        {
            Color color = CurrentPlayer.ChooseColor();
            Utilities.PrintConsoleAlert($"Player chose color {color}");
            return color;
        }

        public void SwitchCardsWithDirectionCard(bool isDirectionNormal)
        {
            Player first = CurrentPlayer;
            List<Card> savedCards = first.PlayerCards;
            first.PlayerCards = [];
            NextPlayer(isDirectionNormal);
            while (!players.First().Equals(first))
            {
                (savedCards, players.First().PlayerCards) = (players.First().PlayerCards, savedCards);
                NextPlayer(isDirectionNormal);
            }
            first.PlayerCards = savedCards;
        }

        internal int RemoveWinner(bool isDirectionNormal)
        {
            Player savedPlayer = CurrentPlayer;
            NextPlayer(isDirectionNormal);
            players.Remove(savedPlayer);
            return savedPlayer.Id;
        }
    }
}
