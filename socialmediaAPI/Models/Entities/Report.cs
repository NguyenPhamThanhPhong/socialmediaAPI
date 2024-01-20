namespace socialmediaAPI.Models.Entities
{
    public class Report
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ReporterId { get; set; }
        public string ReportTime { get; set; }
        public List<string> ReportedUserIds { get; set; }
        public List<string> ReportedPostIds { get; set; }
    }
}
