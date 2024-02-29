using MongoDB.Bson;

namespace TakiApp.Models
{
    internal class Player
    {
        public ObjectId Id { get; set; }
        public List<Card> Cards { get; set; } = [];
        public string? Name { get; set; }
        public string? PlayerAlgorithm { get; set; }
    }
}
