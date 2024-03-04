using MongoDB.Bson;
using Newtonsoft.Json.Linq;

namespace TakiApp.Models
{
    public class Card
    {
        public ObjectId Id { get; set; }
        public string Type { get; set; }
        public string CardColor { get; set; }
        public JObject CardConfigurations { get; set; } = [];

        [Newtonsoft.Json.JsonConstructor]
        public Card(ObjectId id, string type, string cardColor, JObject cardConfigurations)
        {
            Type = type;
            Id = id;
            CardColor = cardColor;
            CardConfigurations = cardConfigurations;
        }

        public Card(string type, string cardColor)
        {
            Id = ObjectId.GenerateNewId();
            Type = type;
            CardColor = cardColor;
        }
    }
}
