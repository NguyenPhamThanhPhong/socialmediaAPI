namespace socialmediaAPI.Models.Embeded.User
{
    public class VerificationTicket
    {
        public string? Code { get; set; }
        public DateTime ExpiredTime { get; set; }
    }
}
