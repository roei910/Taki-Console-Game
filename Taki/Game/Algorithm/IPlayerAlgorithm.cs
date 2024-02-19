using System.Drawing;
using Taki.Game.Cards;
using Taki.Game.Players;

namespace Taki.Game.Algorithm
{
    internal interface IPlayerAlgorithm
    {
        Card? ChooseCard(Func<Card, bool> isSimilarTo, List<Card> playerCards, string? elseMessage = null);
        Color ChooseColor(List<Card> playerCards);
        Player ChoosePlayer(Player currentPlayer, IPlayersHolder playersHolder);
    }
}
