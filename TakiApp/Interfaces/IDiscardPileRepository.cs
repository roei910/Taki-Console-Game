using TakiApp.Models;

namespace TakiApp.Interfaces
{
    public interface IDiscardPileRepository
    {
        Task AddCardAsync(Card card);
        Task DeleteAllAsync();
        Task<Card> GetTopDiscard();
        Task<List<Card>> RemoveCardsForShuffle();
        Task UpdateCardAsync(Card cardToUpdate);
    }
}