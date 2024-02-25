using Taki.Models.Players;

namespace Taki.Dto
{
    internal class PlayerDto
    {
        public int Score { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public List<CardDto> PlayerCards { get; set; }
        public string ChoosingAlgorithm { get; set; }

        public PlayerDto(int score, string name, int id, List<CardDto> playerCards, string choosingAlgorithm)
        {
            Score = score;
            Name = name;
            Id = id;
            PlayerCards = playerCards;
            ChoosingAlgorithm = choosingAlgorithm;
        }
    }
}
