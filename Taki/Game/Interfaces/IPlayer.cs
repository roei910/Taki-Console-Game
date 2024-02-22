using System.Drawing;
using Taki.Game.Models.Cards;
using Taki.Game.Models.Players;

namespace Taki.Game.Interfaces
{
    internal interface IPlayer
    {
        string Name { get; }
        Color ChooseColor();
        Card? PickCard(Func<Card, bool> isSimilarTo, string? elseMessage = null);
        void AddCard(Card card);
        bool IsHandEmpty();
        string GetInformation();
        bool IsManualPlayer();
        Player PickOtherPlayer(IPlayersHolder playersHolder);
    }
}