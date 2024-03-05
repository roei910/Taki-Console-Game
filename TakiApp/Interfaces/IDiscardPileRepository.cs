using TakiApp.Models;

namespace TakiApp.Interfaces
{
    public interface IDiscardPileRepository
    {
        Task AddCardAsync(Card card);
        Task DeleteAllAsync();
        Task<List<Card>> GetCardsOrderedAsync();
        Task<Card> GetTopDiscardAsync();
        Task<List<Card>> RemoveCardsForShuffleAsync();
        Task UpdateCardAsync(Card cardToUpdate);
    }
}