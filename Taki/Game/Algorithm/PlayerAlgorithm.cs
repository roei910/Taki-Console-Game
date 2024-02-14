using System.Drawing;
using Taki.Game.Cards;
using Taki.Game.Messages;

namespace Taki.Game.Algorithm
{
    internal class PlayerAlgorithm : IPlayerAlgorithm
    {
        public virtual Card? ChooseCard(Func<Card, bool> isSimilarTo, List<Card> playerCards, IUserCommunicator userCommunicator)
        {
            if (playerCards.Count == 0)
                return null;

            return playerCards.FirstOrDefault(card => isSimilarTo(card!));
        }


        public Color ChooseColor(List<Card> playerCards, IUserCommunicator userCommunicator)
        {
            var colors = playerCards
                .Where(card => card is ColorCard)
                .Select(card => ((ColorCard)card).GetColor())
                .GroupBy(c => c);

            if (colors.Count() == 0)
                return Color.Blue;

            return colors.OrderByDescending(color => color.Count()).First().FirstOrDefault(Color.Blue);
        }

        public override string ToString()
        {
            return "Player Algo";
        }
    }
}
