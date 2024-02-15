using Taki.Game.Players;

namespace Taki.Game.Cards
{
    internal interface ICard
    {
        int CardsToDraw();
        bool IsStackableWith(Card other);
        void Play(Card topDiscard, IPlayersHolder playersHolder, IServiceProvider serviceProvider);
        void FinishNoPlay();
        void FinishPlay();
    }
}
