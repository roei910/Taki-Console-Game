using Taki.Shared.Interfaces;
using Taki.Shared.Models.Dto;

namespace Taki.Models.Players
{
    internal class PyramidPlayersHolder : PlayersHolder
    {
        public PyramidPlayersHolder(List<Player> players, int numberOfPlayerCards,
            IUserCommunicator userCommunicator, IDal<PlayerDto> playerDatabase) :
            base(players, numberOfPlayerCards, userCommunicator, playerDatabase) { }

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

        protected override bool HasPlayerFinishedHand(ICardDecksHolder cardDecksHolder)
        {
            if (base.HasPlayerFinishedHand(cardDecksHolder))
            {
                PyramidPlayer player = (PyramidPlayer)_players.Where(player => player.IsHandEmpty()).First();

                if (player.CurrentNumberOfCards() > 1)
                {
                    DrawCards(player.GetNextPlayerHand(_userCommunicator), player, cardDecksHolder);
                    _userCommunicator.SendErrorMessage(
                        $"Player: {player.Name}, finished his current hand," +
                        $" currently on {player.CurrentNumberOfCards()} card(s)");

                    return false;
                }

                return true;
            }

            return false;
        }
    }
}
