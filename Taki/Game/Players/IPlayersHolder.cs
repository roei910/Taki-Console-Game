using Taki.Game.Cards;
using Taki.Game.Deck;
using Taki.Game.Messages;

namespace Taki.Game.Players
{
    internal interface IPlayersHolder
    {
        Player CurrentPlayer { get; }
        List<Player> Players { get; }
        void NextPlayer();
        void CurrentPlayerPlay(IServiceProvider serviceProvider);
        void ChangeDirection();
        void DealCards(ICardDecksHolder cardsHolder);
        bool DrawCards(int numberOfCards, ICardDecksHolder cardsHolder, IUserCommunicator userCommunicator);
        Player GetWinner(IServiceProvider serviceProvider);
        List<Card> ReturnCardsFromPlayers();
    }
}