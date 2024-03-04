using System.Drawing;
using TakiApp.Models;

namespace TakiApp.Interfaces
{
    public interface IPlayerService
    {
        Card? PickCard(Player currentPlayer, Card topDiscard);
        void AddCard(Player player, Card card);
        Player PickOtherPlayer(Player current, List<Player> players);
        void PlayCard(Player player, Card card);
        Color ChooseColor(Player player);
    }
}