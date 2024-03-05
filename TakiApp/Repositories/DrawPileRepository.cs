using MongoDB.Driver.Linq;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Repositories
{
    public class DrawPileRepository : IDrawPileRepository
    {
        private readonly IDrawPileDal _drawPileDal;
        private readonly Random _random;

        public DrawPileRepository(IDrawPileDal drawPileDal, Random random)
        {
            _drawPileDal = drawPileDal;
            this._random = random;
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
            var cardsShuffled = cards.OrderBy(x => _random.Next(cards.Count)).ToList();

            await _drawPileDal.CreateManyAsync(cardsShuffled);
        }

        public async Task DeleteAllAsync()
        {
            await _drawPileDal.DeleteAllAsync();
        }

        public async Task<List<Card>> DrawCardsAsync(int cardsToDraw)
        {
            var cards = await _drawPileDal.FindAsync();

            if (cards.Count == 0)
                return [];

            var drawCards = cards.Take(cardsToDraw).ToList();

            await _drawPileDal.DeleteManyAsync(drawCards);

            return drawCards;
        }
    }
}
