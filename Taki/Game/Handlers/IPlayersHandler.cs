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
        bool CanCurrentPlayerPlay();
        void CurrentPlayerPlay(ICardsHandler cardsHandler, IUserCommunicator userCommunicator);
        void ChangeDirection();
        List<Card> GetAllCardsFromPlayers(CardsHandler cardsHandler);
        void DealCards(CardsHandler cardsHandler);
        Player GetCurrentPlayer();
    }
}