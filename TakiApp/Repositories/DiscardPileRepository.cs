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

        public async Task AddCard(Card card)
        {
            var cards = await _discardPileDal.FindAsync();

            if (cards.Count == 0)
            {
                await _discardPileDal.CreateOneAsync(card);

                return;
            }

            await _discardPileDal.DeleteAllAsync();

            await _discardPileDal.CreateOneAsync(card);
            await _discardPileDal.CreateManyAsync(cards);
        }

        public async Task DeleteAllAsync()
        {
            await _discardPileDal.DeleteAllAsync();
        }

        public async Task<Card> GetTopDiscard()
        {
            var cards = await _discardPileDal.FindAsync();

            return cards[0];
        }

        public async Task<List<Card>> RemoveCardsForShuffle()
        {
            var cards = await _discardPileDal.FindAsync();
            await _discardPileDal.DeleteAllAsync();
            await _discardPileDal.CreateOneAsync(cards[0]);

            cards.RemoveAt(0);

            return cards;
        }
    }
}
