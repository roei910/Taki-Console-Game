using MongoDB.Bson;
using Newtonsoft.Json;

namespace TakiApp.Serializers
{
    internal class ObjectIdConverter : Newtonsoft.Json.JsonConverter<ObjectId>
    {
        public override ObjectId ReadJson(JsonReader reader, Type objectType, ObjectId existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                var objectIdString = reader.Value as string;
                return ObjectId.Parse(objectIdString);
            }
            throw new JsonSerializationException($"Unexpected token type: {reader.TokenType}");
        }

        public override void WriteJson(JsonWriter writer, ObjectId value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }
    }
}