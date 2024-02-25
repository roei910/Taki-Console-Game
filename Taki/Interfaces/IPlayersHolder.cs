using Taki.Database;
using Taki.Models.Cards;
using Taki.Models.Players;

namespace Taki.Interfaces
{
    internal interface IPlayersHolder
    {
        Player CurrentPlayer { get; }
        List<Player> Players { get; }
        void NextPlayer();
        void CurrentPlayerPlay(ICardDecksHolder cardDecksHolder);
        void ChangeDirection();
        void DealCards(ICardDecksHolder cardsHolder);
        bool DrawCards(int numberOfCards, Player playerToDraw, ICardDecksHolder cardDecksHolder);
        Player GetWinner(ICardDecksHolder cardDecksHolder);
        List<Card> ReturnCardsFromPlayers();
        void ResetPlayers();
    }
}