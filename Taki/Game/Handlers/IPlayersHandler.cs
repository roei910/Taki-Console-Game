using Taki.Game.Cards;
using Taki.Game.Deck;
using Taki.Game.Messages;
using Taki.Game.Players;

namespace Taki.Game.Handlers
{
    internal interface IPlayersHandler
    {
        bool DrawCards(int numberOfCards, ICardDecksHolder cardsHolder, IUserCommunicator userCommunicator);
        void NextPlayer();
        Player RemoveWinner();
        List<Player> GetAllPlayers();
        bool HasPlayerWon();
        void CurrentPlayerPlay(IServiceProvider serviceProvider);
        void ChangeDirection();
        List<Card> GetAllCardsFromPlayers();
        void DealCards(ICardDecksHolder cardsHolder);
        Player GetCurrentPlayer();
        int CountPlayers();
    }
}