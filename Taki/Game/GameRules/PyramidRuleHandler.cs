using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taki.Game.Deck;
using Taki.Game.General;
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
                if (!playerHandler.DrawCards(player.CurrentNumberOfCards() - 1, cardDeck))
                    throw new Exception("player cannot get more cards and is stuck!");
                player.CurrentCardsMinus1();
                if(player.CurrentNumberOfCards() != 0)
                    Utilities.PrintConsoleError($"Player[{player.Id}] finished his current hand," +
                        $" currently on {player.CurrentNumberOfCards()} card(s)");
                if(CurrentTakiCard != null)
                {
                    CurrentTakiCard = null;
                    playerHandler.NextPlayer(isDirectionNormal);
                }
                return false;
            }
            return false;
        }
    }
}
