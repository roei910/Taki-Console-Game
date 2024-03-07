using MongoDB.Driver.Linq;
using TakiApp.Interfaces;
using TakiApp.Models;

namespace TakiApp.Repositories
{
    public class DrawPileRepository : IDrawPileRepository
    {
        private readonly IDrawPileDal _drawPileDal;
        private readonly Random _random;
        private readonly IDiscardPileRepository _discardPileRepository;

        public DrawPileRepository(IDrawPileDal drawPileDal, Random random,
            IDiscardPileRepository discardPileRepository)
        {
            _drawPileDal = drawPileDal;
            _random = random;
            _discardPileRepository = discardPileRepository;
        }

        public async Task ShuffleCardsAsync(List<Card> cards)
        {
            var cardsShuffled = cards.OrderBy(x => _random.Next(cards.Count)).ToList();

            cardsShuffled = cardsShuffled.Select((card, index) =>
            {
                card.Order = index;
                return card;
            }).ToList();

            await _drawPileDal.CreateManyAsync(cardsShuffled);
        }

        public async Task DeleteAllAsync()
        {
            await _drawPileDal.DeleteAllAsync();
        }

        public async Task<List<Card>> DrawCardsAsync(int cardsToDraw = 1)
        {
            var cards = await _drawPileDal.FindAsync();//TODO: make order in cards

            var drawCards = cards.Take(cardsToDraw).ToList();

            if (drawCards.Count == 0)
            {
                List<Card> discardPile = await _discardPileRepository.GetCardsOrderedAsync();

                var topDiscard = discardPile.First();
                discardPile.Remove(topDiscard);

                if (discardPile.Count == 0)
                    return [];

                await _discardPileRepository.DeleteAllAsync();
                await _discardPileRepository.AddCardOrderedAsync(topDiscard);
                await ShuffleCardsAsync(discardPile);

                cards = await _drawPileDal.FindAsync();

                return cards.Take(cardsToDraw).ToList();
            }

            await _drawPileDal.DeleteManyAsync(drawCards);

            return drawCards;
        }
    }
}
