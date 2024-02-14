using System.Drawing;
using Taki.Game.Cards;
using Taki.Game.Messages;

namespace Taki.Game.Algorithm
{
    internal interface IPlayerAlgorithm
    {
        Card? ChooseCard(Func<Card, bool> isSimilarTo,
            List<Card> playerCards, IUserCommunicator userCommunicator);
        Color ChooseColor(List<Card> playerCards, IUserCommunicator userCommunicator);
    }
}
