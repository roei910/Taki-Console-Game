using Taki.Game.Communicators;
using Taki.Game.Handlers;
using Taki.Game.Players;

namespace Taki.Game.GameRules
{
    internal class PyramidRuleHandler : RuleHandler
    {
        private const int NUMBER_OF_PYRAMID_PLAYER_CARDS = 10;
        public PyramidRuleHandler(GameHandlers gameHandlers) : 
            base(CreatePyramidGameHandler(gameHandlers), NUMBER_OF_PYRAMID_PLAYER_CARDS)
        { }

        private static GameHandlers CreatePyramidGameHandler(GameHandlers gameHandlers)
        {
            var players = gameHandlers.GetPlayersHandler().GetAllPlayers();
            var pyramidPlayers = players.Select(x => (Player)new PyramidPlayer(x)).ToList();

            PlayersHandler pyramidPlayersHandler = new(pyramidPlayers);
            return new(pyramidPlayersHandler,
                gameHandlers.GetCardsHandler(),
                gameHandlers.GetServiceProvider());
        }

        //protected override bool PlayerFinishedHand()
        //{
        //    if (base.PlayerFinishedHand())
        //    {
        //        PyramidPlayer player = (PyramidPlayer)_playersHandler.CurrentPlayer;
        //        if (player.CurrentNumberOfCards() == 0)
        //            return true;
        //        if(player.GetNextPlayerHand() != 0)
        //        {
        //            if (!_playersHandler.DrawCards(player.CurrentNumberOfCards(), cardDeck))
        //                throw new Exception("player cannot get more cards and is stuck!");
        //            MessageHandler.SendMessageToUser($"Player[{player.Id}] finished his current hand," +
        //                $" currently on {player.CurrentNumberOfCards()} card(s)", MessageHandler.MessageType.Error);
        //        }
        //        if(_moveHandler.IsTaki())
        //        {
        //            _moveHandler.CloseTaki();
        //            _playersHandler.NextPlayer(isDirectionNormal);
        //        }
        //        return false;
        //    }
        //    return false;
        //}
    }
}
