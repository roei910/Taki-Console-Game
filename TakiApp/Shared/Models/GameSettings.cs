using MongoDB.Bson;

namespace TakiApp.Shared.Models
{
    public class GameSettings
    {
        public ObjectId Id { get; set; }
        public int NumberOfPlayerCards { get; set; }
        public int NumberOfPlayers { get; set; }
        public int NumberOfManualPlayers { get; set; }
        public string? TypeOfGame { get; set; }
        public bool IsOnline { get; set; }
        public int NumberOfWinners { get; set; }
        public bool HasGameStarted { get; set; } = false;
        public bool HasGameEnded { get; set; } = false;
        public List<string> winners { get; set; } = [];
    }
}
