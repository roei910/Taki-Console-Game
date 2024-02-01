using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taki.Game.Algorithm;
using Taki.Game.Cards;

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
            currentNumberOfCards--;
        }
    }
}
