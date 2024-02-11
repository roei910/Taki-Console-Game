using System.Drawing;
using Taki.Game.Cards;
using Taki.Game.Handlers;
using Taki.Game.Players;

namespace Taki.Game.Interfaces
{
    internal interface IPlayerAlgorithm
    {
        Card? ChooseCard(Func<Card, bool> isSimilarTo,
            Player player,
            GameHandlers gameHandlers);
        Color ChooseColor(GameHandlers gameHandlers);
    }
}
