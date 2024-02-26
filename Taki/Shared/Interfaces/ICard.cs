using Taki.Shared.Abstract;
using Taki.Shared.Models.Dto;

namespace Taki.Shared.Interfaces
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
