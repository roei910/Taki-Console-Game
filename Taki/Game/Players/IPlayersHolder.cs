using Taki.Game.Cards;
using Taki.Game.Deck;

namespace Taki.Game.Players
{
    internal interface IPlayersHolder
    {
        Player CurrentPlayer { get; }
        List<Player> Players { get; }
        void NextPlayer();
        void CurrentPlayerPlay();
        void ChangeDirection();
        void DealCards(ICardDecksHolder cardsHolder);
        bool DrawCards(int numberOfCards, Player playerToDraw);
        Player GetWinner();
        List<Card> ReturnCardsFromPlayers();
    }
}