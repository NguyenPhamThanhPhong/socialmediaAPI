using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using socialmediaAPI.Models.Embeded.User;

namespace socialmediaAPI.Models.Entities
{
    public class MyUser
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }
        public bool IsMailConfirmed { get; set; }
        public AuthenticationInformation AuthenticationInfo { get; set; }
        public PersonalInformation PersonalInfo { get; set; }
        public List<string> FriendWaitIds { get; set; }
        public List<string> FriendRequestIds { get; set; }
        public List<string> FriendIds { get; set; }
        public List<string> BlockedIds { get; set; }
        public List<string> PostIds { get; set; }
        public List<Notification> Notifications { get; set; }
        public VerificationTicket EmailVerification { get; set; }
    }
}
