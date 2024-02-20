using Taki.Game.Deck;
using Taki.Game.Players;

namespace Taki.Game.Cards
{
    internal interface ICard
    {
        int CardsToDraw();
        bool IsStackableWith(Card other);
        void Play(Card topDiscard, ICardDecksHolder cardDecksHolder, IPlayersHolder playersHolder);
        void FinishNoPlay();
        void ResetCard();
        void PrintCard();
        string[] GetStringArray();
    }
}
