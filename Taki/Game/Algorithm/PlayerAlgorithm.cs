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
    internal class PlayerAlgorithm : IPlayerAlgorithm
    {
        protected Communicator communicator = Communicator.GetCommunicator();
        public Card ChooseCard(Card topDeckCard, Player currentPlayer)
        {
            Card card;
            var filter = currentPlayer.PlayerCards
                .Where(card => card.SimilarTo(topDeckCard)).ToList();
            card = filter.FirstOrDefault(topDeckCard);
            if(currentPlayer.PlayerCards.Count != 0)
                Communicator.PrintMessage($"Player[{currentPlayer.Id}] has {currentPlayer.PlayerCards.Count} cards in hand", 
                    Communicator.MessageType.Alert);
            return card;
        }

        public Color ChooseColor(Player currentPlayer)
        {
            var count = currentPlayer.PlayerCards
                .Where(p => p.Color != Color.Empty)
                .GroupBy(p => p.Color).ToList();
            try
            {
                Card card = count.OrderByDescending(v => v.Count()).ToList()
                .First().First();
                Color color = card == null ? Color.Blue : card.Color;
                return color;
            }
            catch
            {
                return Color.Blue;
            }
            
        }

        public Card ChoosePlus2Card(Card topDeckCard, Player currentPlayer)
        {
            Card card = currentPlayer.PlayerCards
                .Where(UniqueCard.IsPlus2).FirstOrDefault(topDeckCard);
            Communicator.PrintMessage($"Player[{currentPlayer.Id}] chose {card}", 
                Communicator.MessageType.Alert);
            return card;
        }

        public override string ToString()
        {
            return "Player Algo";
        }
    }
}
