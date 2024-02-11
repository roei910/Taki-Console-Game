using Taki.Game.Cards;
using Taki.Game.Handlers;
using Taki.Game.Players;

namespace Taki.Game.Algorithm
{
    internal class PlayerHateTakiAlgo : PlayerAlgorithm
    {
        bool takiFlag = false;

        public override Card? ChooseCard(Func<Card, bool> isSimilarTo,
            Player player, GameHandlers gameHandlers)
        {
            Card topDiscard = gameHandlers.GetCardsHandler().GetTopDiscard();
            if (takiFlag)
            {
                takiFlag = false;
                return null;
            }

            if (topDiscard is TakiCard)
                takiFlag = true;

            return base.ChooseCard(isSimilarTo, player, gameHandlers);
        }

        public override string ToString()
        {
            return "Player Hate Taki Algo";
        }
    }
}
