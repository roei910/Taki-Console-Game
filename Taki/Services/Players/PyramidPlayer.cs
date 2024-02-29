using Taki.Shared.Interfaces;
using Taki.Shared.Models.Dto;

namespace Taki.Models.Players
{
    public class PyramidPlayer : Player
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

        public int GetNextPlayerHand(IUserCommunicator userCommunicator)
        {
            userCommunicator.SendAlertMessage($"Player[{Id}]: finished hand {_currentNumberOfCards}");
            return --_currentNumberOfCards;
        }

        public void ResetPyramidPlayerCards(int numberOfPlyerCards)
        {
            _currentNumberOfCards = numberOfPlyerCards;
        }

        public override string ToString()
        {
            return $"Pyramid player: current hand is {_currentNumberOfCards}\n" + base.ToString();
        }

        public override PlayerDto ToPlayerDto()
        {
            var player = base.ToPlayerDto();

            return new PlayerDto(player.Score, player.Name, player.Id, player.PlayerCards,
                player.ChoosingAlgorithm, _currentNumberOfCards);
        }

        public override string GetInformation()
        {
            return $"Pyramid Player Name: {Name}, {PlayerCards.Count} cards in hand\ncurrent number of cards: {_currentNumberOfCards}, Algo: {_choosingAlgorithm}";
        }
    }
}
