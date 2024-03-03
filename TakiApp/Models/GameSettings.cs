using MongoDB.Bson;

namespace TakiApp.Models
{
    public class GameSettings
    {
        public ObjectId Id { get; set; }
        public int NumberOfPlayerCards { get; set; }
        public int NumberOfPlayers { get; set; }
        public string? TypeOfGame { get; set; }
        public bool IsOnline { get; set; }
        public bool HasGameStarted { get; set; }
    }
}
