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
        public bool IsGroup { get; set; }
        public List<string>? Blockers { get; set; }
        public Dictionary<string,string> Nicknames { get; set; }
        public MessageDisplay RecentMessage { get; set; }
        public List<string> MessageIds { get; set; }

        public Conversation()
        {
            ID = string.Empty;
            ParticipantIds = new List<string>();
            IsGroup = false;
            Blockers = new List<string>();
            Nicknames = new Dictionary<string,string>();
            RecentMessage = new MessageDisplay();
            MessageIds = new List<string>();
        }
    }
    public class MessageDisplay
    {
        public string? senderId { get; set; }
        public string? Content { get; set; }
        public DateTime RecentTime { get; set; }
    }
}
