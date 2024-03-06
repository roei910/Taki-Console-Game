using System.Drawing;
using TakiApp.Models;

namespace TakiApp.Interfaces
{
    public interface IAlgorithmService
    {
        Card? PickCard(Player currentPlayer, Func<Card, bool> canStackOnTopDiscard, string? elseMessage = null);
        Player PickOtherPlayer(Player current, List<Player> players);
        Color ChooseColor(Player player);
    }
}