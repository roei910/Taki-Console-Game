using System.Drawing;
using Taki.Game.Cards;

namespace Taki.Game.Players
{
    internal class PlayerDTO
    {
        public int Score { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public List<CardDTO> PlayerCards { get; set; }
        public string ChoosingAlgorithm { get; set; }

        public PlayerDTO(int score, string name, int id, List<CardDTO> playerCards, string choosingAlgorithm)
        {
            Score = score;
            Name = name;
            Id = id;
            PlayerCards = playerCards;
            ChoosingAlgorithm = choosingAlgorithm;
        }

        public static PlayerDTO PlayerToDTO(Player player)
        {
            List<CardDTO> cards = player.PlayerCards.Select(CardDTO.CardToDTO).ToList();
            
            return new PlayerDTO(player.Score, player.Name, player.Id, cards, player.ChoosingAlgorithm);
        }
    }
}
