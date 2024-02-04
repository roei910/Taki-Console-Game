using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Taki.Game.GameRules;
using Taki.Game.Players;

namespace Taki.Game.Managers
{
    internal class PyramidGameManager(int numberOfPlayers) 
        : GameManager(numberOfPlayers, NUMBER_OF_PLAYER_CARDS_PYRAMID)
    {
        private static readonly int NUMBER_OF_PLAYER_CARDS_PYRAMID = 10;

        protected override void Initialize(LinkedList<Player> players)
        {
            base.Initialize(players);
            var pyramidPlayers = players.Select(x => new PyramidPlayer(x)).ToList();
            players = [];
            pyramidPlayers.ForEach(x => players.AddLast(x));
            ruleHandler = new PyramidRuleHandler(new PlayerHandler(players), cardDeck);
        }
    }
}
