using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taki.Game.Deck;
using Taki.Game.Players;

namespace Taki.Game.GameRules
{
    internal class PyramidRuleHandler(PlayerHandler playerHandler, CardDeck cardDeck) : RuleHandler(playerHandler, cardDeck)
    {
        protected override bool PlayerFinishedHand()
        {
            if (base.PlayerFinishedHand())
            {
                PyramidPlayer player = (PyramidPlayer)playerHandler.CurrentPlayer;
                if (player.CurrentNumberOfCards() == 0)
                    return true;
                playerHandler.DrawCards(player.CurrentNumberOfCards() - 1, cardDeck);
                player.CurrentCardsMinus1();
                throw new Exception("check if this is working");
                return false;
            }
            return false;
        }
    }
}
