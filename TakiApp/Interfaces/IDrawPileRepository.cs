using TakiApp.Models;

namespace TakiApp.Interfaces
{
    public interface IDrawPileRepository
    {
        Task AddManyRandomAsync(List<Card> cards);
        Task DeleteAllAsync();
        Task<Card?> DrawCardAsync();
        Task<List<Card>> DrawCardsAsync(int cardsToDraw);
    }
}