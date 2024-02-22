using System.Drawing;
using Taki.Models.Cards;
using Taki.Models.Players;

namespace Taki.Interfaces
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