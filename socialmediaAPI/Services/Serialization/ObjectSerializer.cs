using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace socialmediaAPI.Services.Serialization
{
    public class ObjectSerializer : IBsonSerializer<object?>
    {
        public Type ValueType => typeof(object);

        public object? Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var reader = context.Reader;
            var bsonType = reader.GetCurrentBsonType();

            if (bsonType == BsonType.Document)
            {
                var bsonDocument = BsonDocumentSerializer.Instance.Deserialize(context);
                return DeserializeObjectFromBsonDocument(bsonDocument);
            }

            reader.SkipValue();
            return null;
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object? value)
        {
            var writer = context.Writer;
            var bsonDocument = SerializeObjectToBsonDocument(value);
            BsonDocumentSerializer.Instance.Serialize(context, bsonDocument);
        }

        private object? DeserializeObjectFromBsonDocument(BsonDocument bsonDocument)
        {
            // Here, you can implement custom logic to deserialize the object.
            // For simplicity, this example assumes that the object is a dictionary.

            var dictionary = new Dictionary<string, object?>();

            foreach (var element in bsonDocument)
            {
                dictionary[element.Name] = BsonTypeMapper.MapToDotNetValue(element.Value);
            }

            return dictionary;
        }

        private BsonDocument SerializeObjectToBsonDocument(object? value)
        {
            // Here, you can implement custom logic to serialize the object.
            // For simplicity, this example assumes that the object is a dictionary.

            var dictionary = (Dictionary<string, object?>)value!;

            var bsonDocument = new BsonDocument();

            foreach (var kvp in dictionary)
            {
                bsonDocument.Add(new BsonElement(kvp.Key, BsonTypeMapper.MapToBsonValue(kvp.Value)));
            }

            return bsonDocument;
        }
    }
}
