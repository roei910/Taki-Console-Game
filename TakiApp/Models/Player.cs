using MongoDB.Bson;

namespace TakiApp.Models
{
    public class Player
    {
        public ObjectId Id { get; set; }
        public int Order { get; set; }
        public string? Name { get; set; }
        public string? PlayerAlgorithm { get; set; }
        public DateTime? LastCheckIn { get; set; }
        public bool IsPlaying { get; set; } = false;
        public int Score { get; set; } = 0;
        public List<Card> Cards { get; set; } = [];
    }
}
