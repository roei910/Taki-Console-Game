using MongoDB.Bson;

namespace TakiApp.Models
{
    public class GameSettings
    {
        public ObjectId Id { get; set; }

        public int NumberOfPlayerCards { get; set; }

        public string? TypeOfGame { get; set; }
    }
}
