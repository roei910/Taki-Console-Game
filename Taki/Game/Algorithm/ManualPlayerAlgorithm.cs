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
        protected Communicator communicator = Communicator.GetCommunicator();

        public Card ChooseCard(Card topDeckCard, Player currentPlayer)
        {
            Communicator.PrintMessage($"The top deck card is {topDeckCard}", Communicator.MessageType.Alert);
            currentPlayer.PlayerCards = currentPlayer.PlayerCards
                .GroupBy(card => card.Color).ToList().SelectMany(x => x).ToList();
            Communicator.PrintMessage(currentPlayer);
            Communicator.PrintMessage($"Please choose on of your cards by index, -1 to draw a card", 
                Communicator.MessageType.Alert);
            int index;
            while (!int.TryParse(Communicator.ReadMessage(), out index) || 
                !IsValidIndex(index, currentPlayer.PlayerCards.Count))
                Communicator.PrintMessage("please choose again the index of the card");
            if (index == -1)
                return topDeckCard;
            return currentPlayer.PlayerCards.ElementAt(index);
        }

        public Color ChooseColor(Player currentPlayer)
        {
            Color changeColor = Utilities.GetColorFromUserEnum<CardColorsEnum>("of color");
            Communicator.PrintMessage($"Please choose a card with the new color: {changeColor}",
                Communicator.MessageType.Alert);
            return changeColor;
        }

        public Card ChoosePlus2Card(Card topDiscardPileCard, Player player)
        {
            return ChooseCard(topDiscardPileCard, player);
        }

        public override string ToString()
        {
            return "Manual Player Algo";
        }

        private static bool IsValidIndex(int index, int maxCards)
        {
            return index >= -1 && index < maxCards;
        }

    }
}
