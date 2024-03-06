using TakiApp.Models;

namespace TakiApp.Interfaces
{
    public interface IDiscardPileDal : IDal<Card>
    {
        Task<List<Card>> GetOrderedCardsAsync();
    }
}
