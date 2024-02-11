using System.Diagnostics;

namespace Taki.Game.Players
{
    internal class PyramidPlayer(Player player) : Player(player)
    {
        private int currentNumberOfCards = player.PlayerCards.Count;

        public int CurrentNumberOfCards()
        {
            return currentNumberOfCards;
        }

        public int GetNextPlayerHand()
        {
            //TODO: normal message
            Debug.WriteLine($"Player[{Id}]: finished hand {currentNumberOfCards}");
            return --currentNumberOfCards;
        }

        public override string ToString()
        {
            return $"Pyramid player: current hand is {currentNumberOfCards}\n" + base.ToString();
        }
    }
}
