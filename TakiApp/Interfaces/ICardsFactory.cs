using TakiApp.Models;

namespace TakiApp.Interfaces
{
    public interface ICardsFactory
    {
        List<Card> GenerateDeck();
    }
}