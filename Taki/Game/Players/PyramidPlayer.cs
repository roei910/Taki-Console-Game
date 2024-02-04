using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taki.Game.Algorithm;
using Taki.Game.Cards;
using Taki.Game.General;

namespace Taki.Game.Players
{
    internal class PyramidPlayer(Player player) : Player(player)
    {
        private int currentNumberOfCards = player.PlayerCards.Count;

        public int CurrentNumberOfCards()
        {
            return currentNumberOfCards;
        }

        public void CurrentCardsMinus1()
        {
            Debug.WriteLine($"Player[{Id}]: finished hand {currentNumberOfCards}");
            currentNumberOfCards--;
        }

        public override string ToString()
        {
            return $"Pyramid player: current hand is {currentNumberOfCards}\n" + base.ToString();
        }
    }
}
