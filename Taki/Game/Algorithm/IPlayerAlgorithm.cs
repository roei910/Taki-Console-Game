using System;
using System.Collections.Generic;
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
    internal interface IPlayerAlgorithm
    {
        Card ChooseCard(Card topDeckCard, Player currentPlayer);
        Color ChooseColor(Player currentPlayer);
    }
}
