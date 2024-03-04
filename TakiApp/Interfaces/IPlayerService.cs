using System.Drawing;
using TakiApp.Models;

namespace TakiApp.Interfaces
{
    public interface IPlayerService
    {
        Card? PickCard(Player currentPlayer, Func<Card, bool> canStackOnTopDiscard);
        void AddCard(Player player, Card card);
        Player PickOtherPlayer(Player current, List<Player> players);
        Color ChooseColor(Player player);
    }
}