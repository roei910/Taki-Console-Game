﻿using TakiApp.Interfaces;
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

        public async Task AddCardAsync(Card card)
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

        public async Task<List<Card>> GetCardsOrderedAsync()
        {
            var cards = await _discardPileDal.FindAsync();

            return cards;
        }

        public async Task<Card> GetTopDiscardAsync()
        {
            var cards = await _discardPileDal.FindAsync();

            return cards[0];
        }

        public async Task<List<Card>> RemoveCardsForShuffleAsync()
        {
            var cards = await _discardPileDal.FindAsync();
            await _discardPileDal.DeleteAllAsync();
            await _discardPileDal.CreateOneAsync(cards[0]);

            cards.RemoveAt(0);

            return cards;
        }

        public async Task UpdateCardAsync(Card cardToUpdate)
        {
            await _discardPileDal.UpdateOneAsync(cardToUpdate);
        }
    }
}
