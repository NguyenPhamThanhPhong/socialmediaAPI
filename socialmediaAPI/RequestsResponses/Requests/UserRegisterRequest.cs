using socialmediaAPI.Models.Embeded.User;
using socialmediaAPI.Models.Entities;

namespace socialmediaAPI.RequestsResponses.Requests
{
    public class UserRegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public IFormFile File { get; set; }

        public User ConvertToUser()
        {
            return new User()
            {
                AuthenticationInfo = new AuthenticationInformation()
                {
                    Username = Username,
                    Password = Password,
                    Email = Email,
                    IsVerified = false
                }
            };
        }
    }
}
