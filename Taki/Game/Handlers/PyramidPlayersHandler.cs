using Taki.Game.GameRules;
using Taki.Game.Players;

namespace Taki.Game.Handlers
{
    internal class PyramidPlayersHandler : PlayersHandler
    {
        //TODO: maybe save instances of all handlers inside the code????
        public PyramidPlayersHandler(List<Player> players) : 
            base(CreatePyramidPlayers(players)) { }

        private static List<Player> CreatePyramidPlayers(List<Player> players)
        {
            return players.Select(x => (Player)new PyramidPlayer(x)).ToList();
        }

        public override void CurrentPlayerPlay(GameHandlers gameHandlers)
        {
            base.CurrentPlayerPlay(gameHandlers);

            if (CurrentPlayer.IsHandEmpty())
            {
                PyramidPlayer player = (PyramidPlayer)CurrentPlayer;
                if(player.CurrentNumberOfCards() != 0)
                {
                    DrawCards(player.GetNextPlayerHand(), gameHandlers.GetCardsHandler());
                    gameHandlers.GetMessageHandler().SendErrorMessage(
                        $"Player[{player.Id}] finished his current hand," +
                        $" currently on {player.CurrentNumberOfCards()} card(s)");
                }
            }
        }
    }
}
