using TakiApp.Shared.Models;

namespace TakiApp.Shared.Interfaces
{
    public interface ICardPlayService
    {
        Task PlayCardAsync(Player player, Card cardPlayed);
        Func<Card, bool> CanStack(Card topDiscard);
        int CardsToDraw(Card topDiscard);
        Task FinishNoPlayAsync(Card topDiscard);
        Task FinishPlayAsync(Card cardToReset);
        List<Card> GenerateCardsDeck();
    }
}