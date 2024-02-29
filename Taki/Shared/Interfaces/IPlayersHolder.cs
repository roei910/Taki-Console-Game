using Taki.Models.Players;
using Taki.Shared.Abstract;

namespace Taki.Shared.Interfaces
{
    public interface IPlayersHolder
    {
        Player CurrentPlayer { get; }
        List<Player> Players { get; }
        int NumberOfPlayerCards { get; }
        void NextPlayer();
        void ChangeDirection();
        void DealCards(ICardDecksHolder cardsHolder);
        bool DrawCards(int numberOfCards, Player playerToDraw, ICardDecksHolder cardDecksHolder);
        Player GetWinner(ICardDecksHolder cardDecksHolder);
        List<Card> ReturnCardsFromPlayers();
        void ResetPlayers();
        Card? GetCardFromCurrentPlayer(ICardDecksHolder cardDecksHolder, Func<Card, bool> isStackableWith,
            string? elseMessage = null);
        void UpdateWinnersFromDb();
    }
}