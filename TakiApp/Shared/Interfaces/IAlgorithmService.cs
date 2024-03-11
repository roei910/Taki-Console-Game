using System.Drawing;
using TakiApp.Shared.Models;

namespace TakiApp.Shared.Interfaces
{
    public interface IAlgorithmService
    {
        Card? PickCard(Player currentPlayer, Func<Card, bool> canStackOnTopDiscard, string? elseMessage = null);
        Player PickOtherPlayer(Player current, List<Player> players);
        Color ChooseColor(Player player);
    }
}