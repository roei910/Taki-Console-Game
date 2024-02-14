using System.Drawing;
using Taki.Game.Cards;
using Taki.Game.Handlers;
using Taki.Game.Messages;

namespace Taki.Game.Players
{
    internal interface IPlayer
    {
        Color ChooseColor(IPlayersHandler playersHandler, IUserCommunicator userCommunicator);
        void AddCard(Card card);
        bool IsHandEmpty();
        Card? PickCard(Func<Card, bool> isSimilarTo, IPlayersHandler playersHandler, ICardsHandler cardsHandler, IUserCommunicator userCommunicator);
        string GetInformation();
        string GetName();
    }
}