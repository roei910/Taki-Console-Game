using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taki.Game.Cards;
using Taki.Game.General;
using Taki.Game.Players;

namespace Taki.Game.Algorithm
{
    internal class PlayerHateTakiAlgo : PlayerAlgorithm
    {
        bool takiFlag = false;
        public new Card ChooseCard(Card topDeckCard, Player currentPlayer)
        {
            if(takiFlag)
            {
                takiFlag = false;
                return topDeckCard;
            }
            if (UniqueCard.IsTaki(topDeckCard) || UniqueCard.IsSuperTaki(topDeckCard))
                takiFlag = true;
            return base.ChooseCard(topDeckCard, currentPlayer);
        }

        public override string ToString()
        {
            return "Player Hate Taki Algo";
        }
    }
}
