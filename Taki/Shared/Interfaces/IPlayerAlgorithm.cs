using System.Drawing;
using Taki.Models.Players;
using Taki.Shared.Abstract;

namespace Taki.Shared.Interfaces
{
    public interface IPlayerAlgorithm
    {
        Card? ChooseCard(Func<Card, bool> isSimilarTo, List<Card> playerCards, string? elseMessage = null);
        Color ChooseColor(List<Card> playerCards);
        Player ChoosePlayer(Player currentPlayer, IPlayersHolder playersHolder);
    }
}
