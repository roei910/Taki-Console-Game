using Taki.Game.Handlers;

namespace Taki.Game.Cards
{
    internal interface ICard
    {
        int CardsToDraw();
        bool IsStackableWith(Card other);
        void Play(Card topDiscard, IPlayersHandler playersHandler, IServiceProvider serviceProvider);
        void FinishNoPlay();
        void FinishPlay();
    }
}
