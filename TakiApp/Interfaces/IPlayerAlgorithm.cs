using System.Drawing;
using TakiApp.Models;

namespace TakiApp.Interfaces
{
    public interface IPlayerAlgorithm
    {
        Card? ChooseCard(Func<Card, bool> isSimilarTo, List<Card> playerCards, string? elseMessage = null);
        Color ChooseColor(List<Card> playerCards);
        Player ChoosePlayer(List<Player> players);
    }
}
