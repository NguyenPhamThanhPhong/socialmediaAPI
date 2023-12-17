using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace socialmediaAPI.Models.Entities
{
    public class Conversation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }
        public List<string>? adminID { get; set; }
        public List<string>? ParticipantIds { get; set; }
        public bool IsGroup { get; set; }
        public List<string>? Blockers { get; set; }
        public Dictionary<string,string> Nicknames { get; set; }
        public MessageDisplay RecentMessage { get; set; }
        public List<string> MessageLogIds { get; set; }

        public Conversation()
        {
            ID = string.Empty;
            adminID = new List<string>();
            ParticipantIds = new List<string>();
            Blockers = new List<string>();
            Nicknames = new Dictionary<string,string>();
            RecentMessage = new MessageDisplay();
            MessageLogIds = new List<string>();
        }

    }
    public class MessageDisplay
    {
        public string? senderId { get; set; }
        public string? Content { get; set; }
        public DateTime RecentTime { get; set; }
    }
}
