using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Taki.Game.Serializers
{
    internal class DictionaryBsonSerializer : SerializerBase<Dictionary<string, JObject>>
    {
        public override Dictionary<string, JObject> Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var bsonDocument = BsonDocumentSerializer.Instance.Deserialize(context);
            var json = bsonDocument.ToJson();
            return JsonConvert.DeserializeObject<Dictionary<string, JObject>>(json) ?? [];
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Dictionary<string, JObject> value)
        {
            var bsonDocument = BsonDocument.Parse(JsonConvert.SerializeObject(value));
            BsonDocumentSerializer.Instance.Serialize(context, bsonDocument);
        }
    }
}
