using Taki.Game.Handlers;
using Taki.Game.Messages;

namespace Taki.Game.Cards
{
    internal interface ICard
    {
        int CardsToDraw();
        bool IsStackableWith(Card other);
        void Play(IPlayersHandler playersHandler, ICardsHandler cardsHandler, IUserCommunicator userCommunicator);
        void FinishNoPlay();
        void FinishPlay();
    }
}
