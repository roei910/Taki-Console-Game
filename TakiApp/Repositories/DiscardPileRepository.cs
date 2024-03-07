using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Repositories
{
    public class DiscardPileRepository : IDiscardPileRepository
    {
        private readonly IDiscardPileDal _discardPileDal;

        public DiscardPileRepository(IDiscardPileDal discardPileDal)
        {
            _discardPileDal = discardPileDal;
        }

        public async Task AddCardOrderedAsync(Card card)
        {
            var cards = await _discardPileDal.FindAsync();
            card.Order = cards.Count;

            await _discardPileDal.CreateOneAsync(card);
        }

        public async Task DeleteAllAsync()
        {
            await _discardPileDal.DeleteAllAsync();
        }

        public async Task<List<Card>> GetCardsOrderedAsync()
        {
            var cards = await _discardPileDal.FindAsync();
            cards = cards.OrderByDescending(x => x.Order).ToList();

            return cards;
        }

        public async Task<Card> GetTopDiscardAsync()
        {
            var cards = await GetCardsOrderedAsync();

            return cards[0];
        }

        public async Task UpdateCardAsync(Card cardToUpdate)
        {
            await _discardPileDal.UpdateOneAsync(cardToUpdate);
        }
    }
}
