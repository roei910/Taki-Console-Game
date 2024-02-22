using System.Drawing;
using Taki.Models.Cards;
using Taki.Models.Players;

namespace Taki.Interfaces
{
    internal interface IPlayerAlgorithm
    {
        Card? ChooseCard(Func<Card, bool> isSimilarTo, List<Card> playerCards, string? elseMessage = null);
        Color ChooseColor(List<Card> playerCards);
        Player ChoosePlayer(Player currentPlayer, IPlayersHolder playersHolder);
    }
}
