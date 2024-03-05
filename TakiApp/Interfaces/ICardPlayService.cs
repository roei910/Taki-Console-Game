using TakiApp.Models;

namespace TakiApp.Interfaces
{
    public interface ICardPlayService
    {
        Task PlayCardAsync(Player player, Card cardPlayed);
        Func<Card, bool> CanStack(Card topDiscard);
    }
}