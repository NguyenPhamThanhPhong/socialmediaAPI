namespace socialmediaAPI.Models.Embeded.User
{
    public class AuthenticationInformation
    {
#pragma warning disable CS8618
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool? IsVerified { get; set; }
    }
}
