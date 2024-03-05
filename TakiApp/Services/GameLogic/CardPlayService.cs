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

        public async Task PlayCardAsync(Player player, Card cardPlayed)
        {
            ICardService cardService = MatchCardService(cardPlayed);
            await cardService.PlayAsync(player, cardPlayed, this);
        }

        private ICardService MatchCardService(Card card)
        {
            var found = _cardServices.Where(service => service.ToString() == card.Type.Split(':')[0]).FirstOrDefault();

            return found ?? throw new Exception("couldnt find the card service in the list");
        }
    }
}
