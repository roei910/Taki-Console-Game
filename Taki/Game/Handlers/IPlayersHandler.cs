using Taki.Game.Cards;
using Taki.Game.GameRules;
using Taki.Game.Messages;
using Taki.Game.Players;

namespace Taki.Game.Handlers
{
    internal interface IPlayersHandler
    {
        bool DrawCards(int numberOfCards, ICardsHandler cardsHandler, IUserCommunicator userCommunicator);
        void NextPlayer();
        Player RemoveWinner();
        List<Player> GetAllPlayers();
        bool HasPlayerWon();
        void CurrentPlayerPlay(IServiceProvider serviceProvider);
        void ChangeDirection();
        List<Card> GetAllCardsFromPlayers();
        void DealCards(ICardsHandler cardsHandler);
        Player GetCurrentPlayer();
        int CountPlayers();
    }
}