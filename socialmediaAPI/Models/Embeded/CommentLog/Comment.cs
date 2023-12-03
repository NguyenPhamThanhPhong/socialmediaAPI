namespace socialmediaAPI.Models.Embeded.CommentLog
{
    public class Comment
    {
        public string UserId { get; set; }
        public DateTime CommentTime { get; set; }
        public string Content { get; set; }
        public string? ReplyTo { get; set; } // @nguyenvanA
        public List<string>? ChildCommentIds { get; set; }

        public Comment()
        {
            UserId = string.Empty;
            CommentTime = DateTime.UtcNow;
            Content = string.Empty;
        }
    }
}
