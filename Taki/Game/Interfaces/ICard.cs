using Taki.Game.Cards;
using Taki.Game.GameRules;
using Taki.Game.Handlers;

namespace Taki.Game.Interfaces
{
    internal interface ICard
    {
        int CardsToDraw();
        bool IsSimilarTo(Card other);
        void Play(Card topDiscard, GameHandlers gameHandlers);
        void FinishNoPlay();
        void FinishPlay();
    }
}
