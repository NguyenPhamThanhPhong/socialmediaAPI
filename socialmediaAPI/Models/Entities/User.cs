using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;
using socialmediaAPI.Models.Embeded.User;
using System.Linq.Expressions;

//#pragma warning disable CS8618

namespace socialmediaAPI.Models.Entities
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }
        public bool IsMailConfirmed { get; set; }
        public string Role { get; set; }
        public AuthenticationInformation AuthenticationInfo { get; set; }
        public PersonalInformation PersonalInfo { get; set; }
        public List<string> FollowerIds { get; set; }
        public List<string> FollowingIds { get; set; }
        public List<string> LikedPostIds { get; set; }
        public List<string> SavedPostIds { get; set; }
        public List<string> PostIds { get; set; }
        public List<string> NotificationIds { get; set; }
        public List<string> ConversationIds { get; set; }
        public List<string> ReportIds { get; set; }
        public VerificationTicket EmailVerification { get; set; }

        public User()
        {
            ID = string.Empty;
            AuthenticationInfo = new AuthenticationInformation();
            PersonalInfo= new PersonalInformation();
            Role = "user";
            FollowerIds= new List<string>();
            FollowingIds = new List<string>();
            LikedPostIds= new List<string>();
            SavedPostIds= new List<string>();
            PostIds = new List<string>();
            NotificationIds = new List<string>();
            ConversationIds = new List<string>();
            ReportIds= new List<string>();
            EmailVerification = new VerificationTicket();
        }
        public static string GetFieldName<T>(Expression<Func<User, T>> expression)
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
