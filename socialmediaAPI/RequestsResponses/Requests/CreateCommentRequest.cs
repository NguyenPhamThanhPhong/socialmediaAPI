using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace socialmediaAPI.RequestsResponses.Requests
{
    public class CreateCommentRequest
    {
        public string? PostId { get; set; }
        public string? ParentId { get; set; }
        public string? UserId { get; set; }
        public string Content { get; set; }
        public List<IFormFile>? Files { get; set; }
        public CreateCommentRequest()
        {
            Content = string.Empty;
        }
    }
}
