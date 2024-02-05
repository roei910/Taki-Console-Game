using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taki.Game.Cards;
using Taki.Game.General;
using Taki.Game.Managers;
using Taki.Game.Players;

namespace Taki.Game.Algorithm
{
    internal class ManualPlayerAlgorithm : IPlayerAlgorithm
    {
        public Card ChooseCard(Card topDeckCard, Player currentPlayer)
        {
            Utilities.PrintConsoleAlert($"The top deck card is {topDeckCard}");
            currentPlayer.PlayerCards = currentPlayer.PlayerCards
                .GroupBy(card => card.Color).ToList().SelectMany(x => x).ToList();
            Console.WriteLine(currentPlayer);
            Utilities.PrintConsoleAlert($"Please choose on of your cards by index, -1 to draw a card");
            int index;
            while (!int.TryParse(Console.ReadLine(), out index) || 
                !IsValidIndex(index, currentPlayer.PlayerCards.Count))
                Console.WriteLine("please choose again the index of the card");
            if (index == -1)
                return topDeckCard;
            return currentPlayer.PlayerCards.ElementAt(index);
        }

        public Color ChooseColor(Player currentPlayer)
        {
            Color changeColor = Utilities.GetColorFromUserEnum<CardColorsEnum>("of color");
            Utilities.PrintConsoleAlert($"Please choose a card with the new color: {changeColor}");
            return changeColor;
        }

        private bool IsValidIndex(int index, int maxCards)
        {
            return index >= -1 && index < maxCards;
        }

        public override string ToString()
        {
            return "Manual Player Algo";
        }

        public Card ChoosePlus2Card(Card topDiscardPileCard, Player player)
        {
            return ChooseCard(topDiscardPileCard, player);
        }
    }
}
