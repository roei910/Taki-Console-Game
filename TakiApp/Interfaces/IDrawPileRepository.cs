using TakiApp.Models;

namespace TakiApp.Interfaces
{
    public interface IDrawPileRepository
    {
        Task ShuffleCardsAsync(List<Card> cards);
        Task DeleteAllAsync();
        Task<List<Card>> DrawCardsAsync(int cardsToDraw = 1);
    }
}