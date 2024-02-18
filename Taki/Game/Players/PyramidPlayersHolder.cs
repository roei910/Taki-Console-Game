using Microsoft.Extensions.DependencyInjection;
using Taki.Game.Deck;
using Taki.Game.Messages;

namespace Taki.Game.Players
{
    internal class PyramidPlayersHolder : PlayersHolder
    {
        public PyramidPlayersHolder(List<Player> players, int numberOfPlayerCards, IServiceProvider serviceProvider) :
            base(players, numberOfPlayerCards, serviceProvider) { }

        protected override bool HasPlayerFinishedHand()
        {
            if (base.HasPlayerFinishedHand())
            {
                IUserCommunicator userCommunicator = _serviceProvider.GetRequiredService<IUserCommunicator>();
                ICardDecksHolder cardsHolder = _serviceProvider.GetRequiredService<ICardDecksHolder>();
                PyramidPlayer player = (PyramidPlayer)_players.Where(player => player.IsHandEmpty()).First();

                if (player.CurrentNumberOfCards() > 1)
                {
                    DrawCards(player.GetNextPlayerHand(userCommunicator), player);
                    userCommunicator.SendErrorMessage(
                        $"Player[{player.Id}] finished his current hand," +
                        $" currently on {player.CurrentNumberOfCards()} card(s)");

                    return false;
                }

                return true;
            }

            return false;
        }

        public override void ResetPlayers()
        {
            _ = _players.Select(player =>
            {
                PyramidPlayer pyramidPlayer = (PyramidPlayer)player;
                pyramidPlayer.ResetPyramidPlayerCards(_numberOfPlayerCards);
                return player;
            }).ToList();

            _ = _winners.ToList().Select(w =>
            {
                PyramidPlayer pyramidPlayer = (PyramidPlayer)w;
                pyramidPlayer.ResetPyramidPlayerCards(_numberOfPlayerCards);
                return w;
            }).ToList();

            base.ResetPlayers();
        }
    }
}
