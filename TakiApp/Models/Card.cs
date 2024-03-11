﻿using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TakiApp.Serializers;

namespace TakiApp.Models
{
    public class Card
    {
        [JsonProperty("_id")] // Map the _id field in JSON to the Id property
        [JsonConverter(typeof(ObjectIdConverter))]
        public ObjectId Id { get; set; }
        public string Type { get; set; }
        public string CardColor { get; set; }
        public int Order { get; set; }
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

        public override string ToString()
        {
            return $"{Type.Split('.').Last()}, {CardColor}";
        }
    }
}
