using Taki.Game.Database;
using Taki.Game.Models.Cards;
using Taki.Game.Models.Players;

namespace Taki.Game.Interfaces
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
        Player GetWinner(ICardDecksHolder cardDecksHolder, TakiGameDatabaseHolder takiGameDatabaseHolder);
        List<Card> ReturnCardsFromPlayers();
        void ResetPlayers();
    }
}