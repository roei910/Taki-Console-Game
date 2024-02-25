using Taki.Models.Cards;

namespace Taki.Models.Algorithm
{
    internal class PlayerHateTakiAlgo : PlayerAlgorithm
    {
        bool IsTaki = false;

        public override Card? ChooseCard(Func<Card, bool> isSimilarTo, List<Card> playerCards, string? elseMessage = null)
        {
            if (IsTaki)
            {
                IsTaki = false;
                return null;
            }

            Card? playerCard = base.ChooseCard(isSimilarTo, playerCards);
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
