using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Services.Cards
{
    public abstract class CardService : ICardService
    {
        protected readonly IDiscardPileRepository _discardPileRepository;
        protected readonly IPlayersRepository _playersRepository;

        public CardService(IDiscardPileRepository discardPileRepository,
            IPlayersRepository playersRepository)
        {
            _discardPileRepository = discardPileRepository;
            _playersRepository = playersRepository;
        }

        public virtual bool CanStackOtherOnThis(Card topDiscard, Card otherCard)
        {
            if (otherCard.CardColor == ColorCard.DEFAULT_COLOR.ToString())
                return true;

            if (topDiscard.CardColor == ColorCard.DEFAULT_COLOR.ToString())
                return true;

            return topDiscard.Type == otherCard.Type;
        }

        public virtual async Task PlayAsync(Player player, Card cardPlayed, ICardPlayService cardPlayService)
        {
            await _playersRepository.NextPlayerAsync();
        }

        public virtual int CardsToDraw(Card cardPlayed) => 1;

        public virtual Task FinishNoPlay(Card cardPlayed) => Task.CompletedTask;

        public virtual Task FinishPlayAsync(Card cardToReset) => Task.CompletedTask;
        
        public abstract List<Card> GenerateCardsForDeck();
    }
}
