using System.Drawing;
using TakiApp.Models;

namespace TakiApp.Interfaces
{
    public interface IPlayerService
    {
        Card? PickCard(Player currentPlayer, Func<Card, bool> canStackOnTopDiscard, string? elseMessage = null);
        Player PickOtherPlayer(Player current, List<Player> players);
        Color ChooseColor(Player player);
        Task DrawCard(Player currentPlayer);
    }
}