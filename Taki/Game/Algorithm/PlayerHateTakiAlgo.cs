using Taki.Game.Cards;
using Taki.Game.Messages;

namespace Taki.Game.Algorithm
{
    internal class PlayerHateTakiAlgo : PlayerAlgorithm
    {
        bool IsTaki = false;

        public override Card? ChooseCard(Func<Card, bool> isSimilarTo, List<Card> playerCards, 
            IUserCommunicator userCommunicator)
        {
            if (IsTaki)
            {
                IsTaki = false;
                return null;
            }

            Card? playerCard = base.ChooseCard(isSimilarTo, playerCards, userCommunicator);
            if (playerCard is TakiCard)
                IsTaki = true;

            return playerCard;
        }

        public override string ToString()
        {
            return "Player Hate Taki Algo";
        }
    }
}
