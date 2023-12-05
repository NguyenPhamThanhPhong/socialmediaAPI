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
            var memberExpression = expression.Body as MemberExpression;

            if (memberExpression == null)
            {
                throw new ArgumentException("Invalid expression. Must be a property access expression.", nameof(expression));
            }

            var stack = new Stack<string>();

            while (memberExpression != null)
            {
                stack.Push(memberExpression.Member.Name);
                memberExpression = memberExpression.Expression as MemberExpression;
            }

            return string.Join(".", stack);
        }
    }
}
