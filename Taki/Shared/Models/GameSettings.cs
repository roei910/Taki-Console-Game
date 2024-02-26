using MongoDB.Bson;

namespace Taki.Shared.Models
{
    internal class GameSettings
    {
        public ObjectId Id { get; set; }

        public int NumberOfPlayerCards { get; set; }

        public string? TypeOfGame { get; set; }
    }
}
