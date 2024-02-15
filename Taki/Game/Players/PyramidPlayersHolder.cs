using Microsoft.Extensions.DependencyInjection;
using Taki.Game.Cards;
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

                if (player.CurrentNumberOfCards() != 0)
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

        public override List<Card> ReturnCardsFromPlayers()
        {
            var cards = base.ReturnCardsFromPlayers();

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
