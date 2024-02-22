using Taki.Dto;
using Taki.Models.Cards;

namespace Taki.Interfaces
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
