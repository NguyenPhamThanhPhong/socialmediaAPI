using MongoDB.Bson.Serialization.Attributes;
using socialmediaAPI.Models.Embeded.Post;
using System.Linq.Expressions;

namespace socialmediaAPI.Models.Entities
{
    public class Post
    {
#pragma warning disable CS8618
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Contennt { get; set; }
        public int CommentCounter { get; set; }
        public string? SharedPost { get; set; }
        public Dictionary<string, string> FileUrls { get; set; }
        public OwnerRepresentation Owner { get; set; }
        public List<LikeRepresentation> Likes { get; set; }
        public List<string> CommentLogIds { get; set; }
        public Post()
        {
            Id = string.Empty;
        }
        public static string GetFieldName<T>(Expression<Func<Post, T>> expression)
        {
            if (expression.Body is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }
            return nameof(expression.Body);
        }

    }
}
