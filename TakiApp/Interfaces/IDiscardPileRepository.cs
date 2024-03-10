using MongoDB.Bson;
using TakiApp.Models;

namespace TakiApp.Interfaces
{
    public interface IDiscardPileRepository
    {
        Task AddCardOrderedAsync(Card card);
        Task DeleteAllAsync();
        Task<Card> GetCardById(ObjectId objectId);
        Task<List<Card>> GetCardsOrderedAsync();
        Task<Card> GetTopDiscardAsync();
        Task UpdateCardAsync(Card cardToUpdate);
    }
}