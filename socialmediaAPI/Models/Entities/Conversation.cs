using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace socialmediaAPI.Models.Entities
{
    public class Conversation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }
        public string? Name { get; set; }
        public string? AvatarUrl { get; set; }
        public List<string>? ParticipantIds { get; set; }
        public string RecentMessage { get; set; }
        public DateTime RecentTime { get; set; }
        public List<string> MessageIds { get; set; }

        public Conversation()
        {
            ID = string.Empty;
            ParticipantIds = new List<string>();
            MessageIds = new List<string>();
            RecentMessage = string.Empty;
            RecentTime = DateTime.UtcNow;
        }
    }
    public class MessageDisplay
    {
        public string? senderId { get; set; }
        public string? Content { get; set; }
        public DateTime RecentTime { get; set; }
    }
}
