using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Services.GameLogic
{
    public class CardPlayService : ICardPlayService
    {
        private readonly List<ICardService> _cardServices;

        public CardPlayService(List<ICardService> cardServices)
        {
            _cardServices = cardServices;
        }

        public Func<Card, bool> CanStack(Card cardToStack)
        {
            var cardService = MatchCardService(cardToStack);
            Func<Card, bool> canStack = (Card card) => cardService.CanStackOtherOnThis(cardToStack, card);
            
            return canStack;
        }

        public int CardsToDraw(Card topDiscard)
        {
            var cardService = MatchCardService(topDiscard);

            return cardService.CardsToDraw(topDiscard);
        }

        public async Task FinishNoPlayAsync(Card topDiscard)
        {
            var cardService = MatchCardService(topDiscard);
            
            await cardService.FinishNoPlay(topDiscard);
        }

        public async Task PlayCardAsync(Player player, Card cardPlayed)
        {
            ICardService cardService = MatchCardService(cardPlayed);
            await cardService.PlayAsync(player, cardPlayed, this);
        }

        public async Task FinishPlayAsync(Card cardToReset)
        {
            var service = MatchCardService(cardToReset);

            await service.FinishPlayAsync(cardToReset);
        }

        private ICardService MatchCardService(Card card)
        {
            var found = _cardServices.Where(service => service.ToString() == card.Type.Split(':')[0]).FirstOrDefault();

            return found ?? throw new Exception("couldnt find the card service in the list");
        }
    }
}
