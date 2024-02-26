using MongoDB.Bson;

namespace Taki.Models.GameLogic
{
    internal class GameSettings
    {
        public ObjectId Id { get; set; }

        public int NumberOfPlayerCards { get; set; }

        public string? TypeOfGame { get; set; }
    }
}
