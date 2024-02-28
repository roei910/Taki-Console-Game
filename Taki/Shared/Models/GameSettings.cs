using MongoDB.Bson;

namespace Taki.Shared.Models
{
    public class GameSettings
    {
        public ObjectId Id { get; set; }

        public int NumberOfPlayerCards { get; set; }

        public string? TypeOfGame { get; set; }
    }
}
