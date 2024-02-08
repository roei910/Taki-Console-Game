using System.Drawing;
using Taki.Game.Cards;
using Taki.Game.Players;

namespace Taki.Game.Interfaces
{
    internal interface IPlayerAlgorithm
    {
        Card ChooseCard(Card topDeckCard, Player currentPlayer);
        Color ChooseColor(Player currentPlayer);
        Card ChoosePlus2Card(Card topDiscardPileCard, Player player);
        void PlayCard();
    }
}
