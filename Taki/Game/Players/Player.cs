﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Taki.Game.Algorithm;
using Taki.Game.Cards;
using Taki.Game.General;
using Taki.Game.Managers;

namespace Taki.Game.Players
{
    internal class Player
    {
        private static int id = 0;
        public List<Card> PlayerCards { get; set; }
        public int Id { get; }
        private readonly IPlayerAlgorithm choosingAlgorithm;

        public Player(IPlayerAlgorithm playerAlgorithm)
        {
            PlayerCards = [];
            Id = id++;
            choosingAlgorithm = playerAlgorithm;
        }

        public Player(Player other)
        {
            PlayerCards = other.PlayerCards;
            Id = other.Id;
            choosingAlgorithm = other.choosingAlgorithm;
        }

        public bool AskPlayerToPickCard(Card topDiscardPileCard, out Card chosenCard)
        {
            chosenCard = choosingAlgorithm.ChooseCard(topDiscardPileCard, this);
            if (chosenCard.Equals(topDiscardPileCard))
                return false;
            TryRemoveCardFromHand(chosenCard);
            return true;
        }

        public Color ChooseColor()
        {
            return choosingAlgorithm.ChooseColor(this);
        }

        public void AddCard(Card card)
        {
            PlayerCards.Add(card);
        }

        public bool IsHandEmpty()
        {
            return PlayerCards.Count == 0;
        }

        private void TryRemoveCardFromHand(Card card)
        {
            try
            {
                PlayerCards.Remove(card);
                if (IsHandEmpty())
                    throw new Exception("you won!");
            }
            catch (Exception e)
            {
                if (IsHandEmpty())
                {
                    Utilities.PrintConsoleError($"Player[{Id}] won");
                    return;
                    throw new Exception("you won!");
                }
                throw new Exception("choosing algorithm error, card not found", e);
            }

        }

        public override string ToString()
        {
            string cardsInHand = string.Join("\n", PlayerCards.Select((x, i) => $"{i}.{x}"));
            string str = $"Player {Id}, algorithm: {choosingAlgorithm}, " +
                $"{PlayerCards.Count} Cards:\n{cardsInHand}";
            return str;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;
            if (obj is not Player)
                return false;
            return id == ((Player)obj).Id;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
}
