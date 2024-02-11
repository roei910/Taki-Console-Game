using Taki.Game.Cards;
using Taki.Game.Handlers;
using Taki.Game.Players;

namespace Taki.Game.Algorithm
{
    internal class PlayerHateTakiAlgo : PlayerAlgorithm
    {
        public override Card? ChooseCard(Func<Card, bool> isSimilarTo,
            Player player, GameHandlers gameHandlers)
        {
            Card topDiscard = gameHandlers.GetCardsHandler().GetTopDiscard();

            if (topDiscard is TakiCard)
                return null;

            return base.ChooseCard(isSimilarTo, player, gameHandlers);
        }

        public override string ToString()
        {
            return "Player Hate Taki Algo";
        }
    }
}
