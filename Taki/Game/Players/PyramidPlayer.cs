using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taki.Game.Algorithm;

namespace Taki.Game.Players
{
    internal class PyramidPlayer(Player player) : Player(player)
    {
        private int currentNumberOfCards = player.PlayerCards.Count;


    }
}
