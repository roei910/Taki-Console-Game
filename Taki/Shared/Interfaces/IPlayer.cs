using System.Drawing;
using Taki.Models.Players;
using Taki.Shared.Abstract;
using Taki.Shared.Models.Dto;

namespace Taki.Shared.Interfaces
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
        public PlayerDto ToPlayerDto();
    }
}