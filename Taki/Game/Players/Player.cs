﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Taki.Game.Algorithm;
using Taki.Game.Cards;
using Taki.Game.General;
using Taki.Game.Managers;

namespace Taki.Game.Players
{
    internal class Player
    {
        private static int id = 0;
        private readonly IPlayerAlgorithm choosingAlgorithm;
        public string Name { get; }
        public int Id { get; }
        public List<Card> PlayerCards { get; set; }

        public Player(string personName, IPlayerAlgorithm playerAlgorithm)
        {
            PlayerCards = [];
            Id = id++;
            choosingAlgorithm = playerAlgorithm ?? throw new ArgumentNullException(nameof(playerAlgorithm));
            Name = personName;
        }

        public Player(Player other)
        {
            PlayerCards = new(other.PlayerCards);
            Id = other.Id;
            choosingAlgorithm = other.choosingAlgorithm;
            Name = other.Name;
        }

        public bool AskPlayerToPickCard(Card topDiscardPileCard, out Card chosenCard)
        {
            chosenCard = choosingAlgorithm.ChooseCard(topDiscardPileCard, this);
            if (chosenCard.Id == topDiscardPileCard.Id)
                return false;
            if (!PlayerCards.Remove(chosenCard))
                throw new Exception("card not found in player cards");
            return true;
        }

        public Color ChooseColor()
        {
            Color color = choosingAlgorithm.ChooseColor(this);
            return color;
        }

        public void AddCard(Card card)
        {
            PlayerCards.Add(card);
        }

        public bool IsHandEmpty()
        {
            return PlayerCards.Count == 0;
        }

        public bool AskPlayerToPickCardPlus2(Card topDiscardPileCard, out Card chosenCard)
        {
            chosenCard = choosingAlgorithm.ChoosePlus2Card(topDiscardPileCard, this);
            if (chosenCard.Id == topDiscardPileCard.Id)
                return false;
            if (!PlayerCards.Remove(chosenCard))
                throw new Exception("card not found in player cards");
            return true;
        }

        public override string ToString()
        {
            string cardsInHand = string.Join("\n", PlayerCards.Select((x, i) => $"{i}.{x}"));
            string str = $"Player[{Id}] {Name}, {PlayerCards.Count} Cards:\n{cardsInHand}";
            return str;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not Player player)
                return false;
            return Id == player.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
