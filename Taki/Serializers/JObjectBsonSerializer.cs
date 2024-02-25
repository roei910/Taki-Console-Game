using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Taki.Serializers
{
    internal class JObjectBsonSerializer : SerializerBase<JObject>
    {
        public override JObject Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var bsonDocument = BsonDocumentSerializer.Instance.Deserialize(context);
            var json = bsonDocument.ToJson();
            JObject obj = JsonConvert.DeserializeObject<JObject>(json) ?? [];

            return JsonConvert.DeserializeObject<JObject>(json) ?? [];
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, JObject value)
        {
            var bsonDocument = BsonDocument.Parse(JsonConvert.SerializeObject(value));
            BsonDocumentSerializer.Instance.Serialize(context, bsonDocument);
        }
    }
}
