namespace Taki.Shared.Models.Dto
{
    public class PlayerDto
    {
        public int Score { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public List<CardDto> PlayerCards { get; set; }
        public string ChoosingAlgorithm { get; set; }
        public int CurrentNumberOfCards { get; set; }

        [Newtonsoft.Json.JsonConstructor]
        public PlayerDto(int score, string name, int id, List<CardDto> playerCards,
            string choosingAlgorithm, int currentNumberOfCards = -1)
        {
            Score = score;
            Name = name;
            Id = id;
            PlayerCards = playerCards;
            ChoosingAlgorithm = choosingAlgorithm;
            CurrentNumberOfCards = currentNumberOfCards;
        }
    }
}
