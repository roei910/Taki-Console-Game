using MongoDB.Bson;
using TakiApp.Shared.Models;

namespace TakiApp.Shared.Interfaces
{
    public interface IDiscardPileRepository
    {
        Task AddCardOrderedAsync(Card card);
        Task DeleteAllAsync();
        Task<Card> GetCardByIdAsync(ObjectId objectId);
        Task<List<Card>> GetCardsOrderedAsync();
        Task<Card> GetTopDiscardAsync();
        Task UpdateCardAsync(Card cardToUpdate);
    }
}