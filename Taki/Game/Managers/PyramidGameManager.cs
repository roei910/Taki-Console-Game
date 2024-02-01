﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taki.Game.Managers
{
    internal class PyramidGameManager(int numberOfPlayers) : 
        GameManager(numberOfPlayers, NUMBER_OF_PLAYER_CARDS_PYRAMID)
    {
        private static readonly int NUMBER_OF_PLAYER_CARDS_PYRAMID = 10;
        //if pyramid number of cards is 10, and counting down
        int numberOfPlayerCards = numberOfPlayers;
    }
}
