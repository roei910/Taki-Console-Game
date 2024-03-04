using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Repositories
{
    public class DrawPileRepository : IDrawPileRepository
    {
        private readonly IDrawPileDal _drawPileDal;

        public DrawPileRepository(IDrawPileDal drawPileDal)
        {
            _drawPileDal = drawPileDal;
        }

        public async Task<Card?> DrawCardAsync()
        {
            var cards = await _drawPileDal.FindAsync();
            var first = cards.FirstOrDefault();

            if(first is null)
                return null;

            await _drawPileDal.DeleteAsync(first);

            return first;
        }

        public async Task AddManyRandomAsync(List<Card> cards)
        {
            var cardsShuffled = cards.OrderBy(x => Guid.NewGuid()).ToList();

            await _drawPileDal.CreateManyAsync(cardsShuffled);
        }

        public async Task DeleteAllAsync()
        {
            await _drawPileDal.DeleteAllAsync();
        }
    }
}
