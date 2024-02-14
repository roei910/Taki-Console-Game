using Taki.Game.Cards;
using Taki.Game.GameRules;
using Taki.Game.Messages;
using Taki.Game.Players;

namespace Taki.Game.Handlers
{
    internal class PyramidPlayersHandler : PlayersHandler
    {
        public PyramidPlayersHandler(List<Player> players, int numberOfPlayerCards) : 
            base(players, numberOfPlayerCards) { }

        public override void CurrentPlayerPlay(ICardsHandler cardsHandler, IUserCommunicator userCommunicator)
        {
            base.CurrentPlayerPlay(cardsHandler, userCommunicator);

            if (CurrentPlayer.IsHandEmpty())
            {
                PyramidPlayer player = (PyramidPlayer)CurrentPlayer;
                if(player.CurrentNumberOfCards() != 0)
                {
                    DrawCards(player.GetNextPlayerHand(userCommunicator), cardsHandler, userCommunicator);
                    userCommunicator.SendErrorMessage(
                        $"Player[{player.Id}] finished his current hand," +
                        $" currently on {player.CurrentNumberOfCards()} card(s)");
                }
            }
        }

        public override List<Card> GetAllCardsFromPlayers(ICardsHandler cardsHandler)
        {
            var cards = base.GetAllCardsFromPlayers(cardsHandler);

            _players.Select(player =>
            {
                PyramidPlayer pyramidPlayer = (PyramidPlayer)player;
                pyramidPlayer.ResetPyramidPlayerCards(_numberOfPlayerCards);
                return player;
            }).ToList();

            return cards;
        }
    }
}
