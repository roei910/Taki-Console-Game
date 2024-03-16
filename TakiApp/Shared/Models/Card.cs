using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TakiApp.Serializers;
using TakiApp.Services.Cards;

namespace TakiApp.Shared.Models
{
    public class Card
    {
        [JsonProperty("_id")]
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }
        public string Type { get; set; }
        public string CardColor { get; set; }
        public int Order { get; set; }
        public JObject CardConfigurations { get; set; } = [];

        [JsonConstructor]
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

        public override string ToString()
        {
            var str = $"{Type.Split('.').Last()}";

            if (CardColor != ColorCard.DEFAULT_COLOR.Name)
                str += $", {CardColor}";
            
            return str;
        }
    }
}
