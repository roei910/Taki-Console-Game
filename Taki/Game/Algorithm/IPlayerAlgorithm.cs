using System.Drawing;
using Taki.Game.Cards;
using Taki.Game.Handlers;
using Taki.Game.Messages;
using Taki.Game.Players;

namespace Taki.Game.Algorithm
{
    internal interface IPlayerAlgorithm
    {
        Card? ChooseCard(Func<Card, bool> isSimilarTo,
            Player player, IPlayersHandler playersHandler, 
            ICardsHandler cardsHandler, IUserCommunicator userCommunicator);
        Color ChooseColor(IPlayersHandler playersHandler, IUserCommunicator userCommunicator);
    }
}
