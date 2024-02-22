using System.Drawing;
using Taki.Game.Models.Cards;
using Taki.Game.Models.Players;

namespace Taki.Game.Interfaces
{
    internal interface IPlayerAlgorithm
    {
        Card? ChooseCard(Func<Card, bool> isSimilarTo, List<Card> playerCards, string? elseMessage = null);
        Color ChooseColor(List<Card> playerCards);
        Player ChoosePlayer(Player currentPlayer, IPlayersHolder playersHolder);
    }
}
