using MongoDB.Bson.Serialization.Attributes;

namespace socialmediaAPI.Models.Entities
{
    public class Report
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ReporterId { get; set; }
        public DateTime ReportTime { get; set; }
        public List<string> ReportedUserIds { get; set; }
        public List<string> ReportedPostIds { get; set; }
        public Report()
        {
            ID = string.Empty;
            Title= string.Empty;
            Content= string.Empty;
            ReporterId= string.Empty;
            ReportTime = DateTime.UtcNow;
            ReportedUserIds = new List<string>();
            ReportedPostIds = new List<string>();

        }
    }
}
