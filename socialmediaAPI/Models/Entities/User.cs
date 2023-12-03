using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using socialmediaAPI.Models.Embeded.User;
using System.Linq.Expressions;

#pragma warning disable CS8618

namespace socialmediaAPI.Models.Entities
{
    [BsonIgnoreExtraElements]
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }
        public AuthenticationInformation AuthenticationInfo { get; set; }
        public PersonalInformation PersonalInfo { get; set; }
        public List<string> FriendWaitIds { get; set; }
        public List<string> FriendRequestIds { get; set; }
        public List<string> FriendIds { get; set; }
        public List<string> BlockedIds { get; set; }
        public List<string> PostIds { get; set; }
        public List<Notification> Notifications { get; set; }
        public VerificationTicket EmailVerification { get; set; }
        public User()
        {
            ID = string.Empty;
        }
        public static string GetFieldName<T>(Expression<Func<User, T>> expression)
        {
            if (expression.Body is MemberExpression memberExpression)
            {
                return memberExpression.Member.Name;
            }
            return nameof(expression.Body);
        }

    }
}
