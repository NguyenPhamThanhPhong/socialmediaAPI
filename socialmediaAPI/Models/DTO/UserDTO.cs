using socialmediaAPI.Models.Embeded.User;

namespace socialmediaAPI.Models.DTO
{
    public class UserDTO
    {
        public string ID { get; set; }
        public AuthenticationInformation AuthenticationInfo { get; set; }
        public PersonalInformation PersonalInfo { get; set; }
        public List<string> FriendIds { get; set; }

    }
}
