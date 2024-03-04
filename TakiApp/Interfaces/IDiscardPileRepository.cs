using TakiApp.Models;

namespace TakiApp.Interfaces
{
    public interface IDiscardPileRepository
    {
        Task AddCard(Card card);
        Task DeleteAllAsync();
        Task<Card> GetTopDiscard();
        Task<List<Card>> RemoveCardsForShuffle();
    }
}