using Taki.Game.Dto;
using Taki.Game.Models.Cards;

namespace Taki.Game.Interfaces
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
        CardDto ToCardDto();
        void UpdateFromDto(CardDto cardDTO, ICardDecksHolder cardDecksHolder);
    }
}
