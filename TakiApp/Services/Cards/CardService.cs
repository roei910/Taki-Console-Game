using TakiApp.Shared.Interfaces;
using TakiApp.Shared.Models;

namespace TakiApp.Services.Cards
{
    public abstract class CardService : ICardService
    {
        protected readonly IDiscardPileRepository _discardPileRepository;
        protected readonly IPlayersRepository _playersRepository;
        protected readonly IUserCommunicator _userCommunicator;

        public CardService(IDiscardPileRepository discardPileRepository,
            IPlayersRepository playersRepository, IUserCommunicator userCommunicator)
        {
            _discardPileRepository = discardPileRepository;
            _playersRepository = playersRepository;
            _userCommunicator = userCommunicator;
        }

        public virtual bool CanStackOtherOnThis(Card topDiscard, Card otherCard, ICardPlayService cardPlayService)
        {
            if (otherCard.CardColor == ColorCard.DEFAULT_COLOR.ToString())
                return true;

            if (topDiscard.CardColor == ColorCard.DEFAULT_COLOR.ToString())
                return true;

            var ans = topDiscard.Type == otherCard.Type;

            if (!ans)
                _userCommunicator.SendErrorMessage($"Previous card was {topDiscard}");

            return ans;
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
