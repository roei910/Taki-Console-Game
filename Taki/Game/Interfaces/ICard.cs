using Taki.Game.Cards;
using Taki.Game.Handlers;

namespace Taki.Game.Interfaces
{
    internal interface ICard
    {
        int CardsToDraw();
        bool IsSimilarTo(Card other);
        void Play(GameHandlers gameHandlers);
        void FinishNoPlay();
        void FinishPlay();
    }
}
