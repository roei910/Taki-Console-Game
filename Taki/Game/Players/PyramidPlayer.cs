using System.Diagnostics;

namespace Taki.Game.Players
{
    internal class PyramidPlayer : Player
    {
        private int _currentNumberOfCards;

        public PyramidPlayer(Player player, int numberOfPlayerCards) : base(player)
        {
            _currentNumberOfCards = numberOfPlayerCards;
        }

        public int CurrentNumberOfCards()
        {
            return _currentNumberOfCards;
        }

        public int GetNextPlayerHand()
        {
            //TODO: normal message
            Debug.WriteLine($"Player[{Id}]: finished hand {_currentNumberOfCards}");
            return --_currentNumberOfCards;
        }

        public override string ToString()
        {
            return $"Pyramid player: current hand is {_currentNumberOfCards}\n" + base.ToString();
        }
    }
}
