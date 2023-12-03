using System.Net.NetworkInformation;

namespace socialmediaAPI.Services.Validators
{
    public class UserValidator
    {
        public async Task<bool> IsValidEmail(string email)
        {
            try
            {
                var address = new System.Net.Mail.MailAddress(email);
                var domain = address.Host;

                // Perform a DNS lookup to check if the domain exists
                var ping = new Ping();
                var reply = await ping.SendPingAsync(domain);
                return reply.Status == IPStatus.Success;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
