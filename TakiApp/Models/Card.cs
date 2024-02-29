using MongoDB.Bson;

namespace TakiApp.Models
{
    internal class Card
    {
        public ObjectId Id { get; set; }
        public string? Type { get; set; }
    }
}
