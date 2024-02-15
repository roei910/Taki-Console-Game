using Microsoft.Extensions.DependencyInjection;
using Taki.Game.Cards;
using Taki.Game.Deck;
using Taki.Game.Messages;

namespace Taki.Game.Players
{
    internal class PyramidPlayersHolder : PlayersHolder
    {
        public PyramidPlayersHolder(List<Player> players, int numberOfPlayerCards) :
            base(players, numberOfPlayerCards)
        { }

        public override void CurrentPlayerPlay(IServiceProvider serviceProvider)
        {
            base.CurrentPlayerPlay(serviceProvider);
            IUserCommunicator userCommunicator = serviceProvider.GetRequiredService<IUserCommunicator>();
            ICardDecksHolder cardsHolder = serviceProvider.GetRequiredService<ICardDecksHolder>();

            if (CurrentPlayer.IsHandEmpty())
            {
                PyramidPlayer player = (PyramidPlayer)CurrentPlayer;
                if (player.CurrentNumberOfCards() != 0)
                {
                    DrawCards(player.GetNextPlayerHand(userCommunicator), cardsHolder, userCommunicator);
                    userCommunicator.SendErrorMessage(
                        $"Player[{player.Id}] finished his current hand," +
                        $" currently on {player.CurrentNumberOfCards()} card(s)");
                }
            }
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
