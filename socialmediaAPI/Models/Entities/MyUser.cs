using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using socialmediaAPI.Models.Embeded.User;
using System.Runtime.Serialization;

namespace socialmediaAPI.Models.Entities
{
    public class MyUser : ISerializable
    {
        public object Value { get; set; }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Value", Value);
        }
    }
}
