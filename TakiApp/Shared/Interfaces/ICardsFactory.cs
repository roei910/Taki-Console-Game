using TakiApp.Shared.Models;

namespace TakiApp.Shared.Interfaces
{
    public interface ICardsFactory
    {
        List<Card> GenerateDeck();
    }
}