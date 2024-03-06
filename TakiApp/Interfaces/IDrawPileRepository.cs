using TakiApp.Models;

namespace TakiApp.Interfaces
{
    public interface IDrawPileRepository
    {
        Task AddManyRandomAsync(List<Card> cards);
        Task DeleteAllAsync();
        Task<List<Card>> DrawCardsAsync(int cardsToDraw = 1);
    }
}