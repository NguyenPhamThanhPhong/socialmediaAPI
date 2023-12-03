using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using socialmediaAPI.Models.Embeded.CommentLog;
using System.Linq.Expressions;

namespace socialmediaAPI.Models.Entities
{
    public class CommentLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string PostId { get; set; }
        public string? ParentCommentId { get; set; }
        public ushort Counter { get; set; }
        public ushort Chapter { get; set; }
        public List<Comment> Comments { get; set; }

        public CommentLog()
        {
            Id = string.Empty;
            PostId = string.Empty;
            Comments = new List<Comment>();
        }
        public static string GetFieldName<T>(Expression<Func<CommentLog, T>> expression)
        {
            if (expression.Body is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }
            return nameof(expression.Body);
        }
    }
}
