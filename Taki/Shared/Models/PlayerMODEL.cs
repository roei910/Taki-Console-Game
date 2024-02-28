using MongoDB.Bson;
using Taki.Shared.Abstract;

namespace Taki.Shared.Models
{
    public class PlayerMODEL
    {
        public ObjectId Id { get; set; } = new ObjectId();
        public int Score { get; set; }
        public string? Name { get; set; }
        public string? ChoosingAlgorithm { get; set; }
        public List<Card> PlayerCards { get; set; } = [];
        public int NumberOfPlayerCards { get; set; }
    }
}
