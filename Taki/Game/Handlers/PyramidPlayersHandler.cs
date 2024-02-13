using Taki.Game.GameRules;
using Taki.Game.Players;

namespace Taki.Game.Handlers
{
    internal class PyramidPlayersHandler : PlayersHandler
    {
        //TODO: need to move dealing cards here to be able to control dealing cards for pyramid players
        public PyramidPlayersHandler(List<Player> players, int numberOfPlayerCards) : 
            base(players, numberOfPlayerCards) { }

        public override void CurrentPlayerPlay(GameHandlers gameHandlers)
        {
            base.CurrentPlayerPlay(gameHandlers);

            if (CurrentPlayer.IsHandEmpty())
            {
                PyramidPlayer player = (PyramidPlayer)CurrentPlayer;
                if(player.CurrentNumberOfCards() != 0)
                {
                    DrawCards(player.GetNextPlayerHand(), gameHandlers);
                    gameHandlers.GetMessageHandler().SendErrorMessage(
                        $"Player[{player.Id}] finished his current hand," +
                        $" currently on {player.CurrentNumberOfCards()} card(s)");
                }
            }
        }
    }
}
