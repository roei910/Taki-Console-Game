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
        public Card ChooseCard(Card topDeckCard, Player currentPlayer)
        {
            Card card;
            var filter = currentPlayer.PlayerCards
                .Where(card => card.SimilarTo(topDeckCard)).ToList();
            Debug.WriteLine(currentPlayer.ToString());
            if(UniqueCard.IsPlus2(topDeckCard))
            {
                card = filter.Where(card => card.Equals(topDeckCard))
                    .FirstOrDefault(topDeckCard);
                Utilities.PrintConsoleAlert($"Player[{currentPlayer.Id}] chose {card}");
                return card;
            }
            card = filter.FirstOrDefault(topDeckCard);
            Utilities.PrintConsoleAlert($"Player[{currentPlayer.Id}] has {currentPlayer.PlayerCards.Count} cards in hand");
            return card;
        }

        public Color ChooseColor(Player currentPlayer)
        {
            Utilities.PrintConsoleError("from player");
            var count = currentPlayer.PlayerCards
                .Where(p => p.Color != Color.Empty)
                .GroupBy(p => p.Color).ToList();

            return count.OrderByDescending(v => v.Count()).ToList()
                .First().FirstOrDefault(new NumberCard("", Color.Green)).Color;
        }
    }
}
