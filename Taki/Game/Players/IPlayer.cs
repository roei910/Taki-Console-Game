using System.Drawing;
using Taki.Game.Cards;

namespace Taki.Game.Players
{
    internal interface IPlayer
    {
        Color ChooseColor();
        Card? PickCard(Func<Card, bool> isSimilarTo);
        void AddCard(Card card);
        bool IsHandEmpty();
        string GetInformation();
        string GetName();
        bool IsManualPlayer();
    }
}