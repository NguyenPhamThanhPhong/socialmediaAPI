using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace socialmediaAPI.RequestsResponses.Requests
{
    public class CreateCommentRequest
    {
        public string? PostId { get; set; }
        public string? ParentId { get; set; }
        public string? UserId { get; set; }
        public DateTime? CommentTime { get; set; }
        public string? Content { get; set; }
        public CreateCommentRequest()
        {

        }
    }
}
