using System.Drawing;
using Taki.Game.Cards.DTOs;

namespace Taki.Game.Players
{
    internal class PlayerDTO
    {
        public int Score { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public List<CardDto> PlayerCards { get; set; }
        public string ChoosingAlgorithm { get; set; }

        public PlayerDTO(int score, string name, int id, List<CardDto> playerCards, string choosingAlgorithm)
        {
            Score = score;
            Name = name;
            Id = id;
            PlayerCards = playerCards;
            ChoosingAlgorithm = choosingAlgorithm;
        }

        public static PlayerDTO PlayerToDTO(Player player)
        {
            List<CardDto> cards = player.PlayerCards.Select(card => card.ToCardDto()).ToList();
            
            return new PlayerDTO(player.Score, player.Name, player.Id, cards, player.ChoosingAlgorithm);
        }
    }
}
