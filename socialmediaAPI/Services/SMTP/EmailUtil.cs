using socialmediaAPI.Configs;
using System.Net;
using System.Net.Mail;

namespace socialmediaAPI.Services.SMTP
{
    public class EmailUtil
    {
        private readonly SMTPConfigs _SMTPConfigs;

        public EmailUtil(SMTPConfigs sMTPConfigs)
        {
            _SMTPConfigs = sMTPConfigs;
        }

        public async Task<bool> SendEmailAsync(string toEmail, string titleEmail, string contentEmail)
        {
            // Thay đổi thông tin tài khoản Gmail của bạn tại đây
            string senderEmail = "phong741258963@gmail.com";
            string password = "pvdpwkkcyoniutve";

            // Cấu hình thông tin kết nối SMTP
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(senderEmail, password);

            // Tạo đối tượng MailMessage để cấu hình email
            MailMessage mailMessage = new MailMessage(senderEmail, toEmail, titleEmail,contentEmail);
            mailMessage.IsBodyHtml = true;

            try
            {
                // Gửi email
                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error is");
                Console.WriteLine(ex.Message);
                Console.WriteLine("end ------------");
                return false;
            }
        }

        public async Task<bool> SendEmail(string recipientEmail, string subject, string body)
        {
            // Thay đổi thông tin tài khoản Gmail của bạn tại đây
            string senderEmail = "21522458@gm.uit.edu.vn";
            string password = "xgkwgffeasppvkso\r\n";

            // Cấu hình thông tin kết nối SMTP
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(senderEmail, password);

            // Tạo đối tượng MailMessage để cấu hình email
            MailMessage mailMessage = new MailMessage(senderEmail, recipientEmail, subject, body);
            mailMessage.IsBodyHtml = true;

            try
            {
                // Gửi email
                await smtpClient.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error is");
                Console.WriteLine(ex.Message);
                Console.WriteLine("end ------------");
                return false;
            }
        }
    }
}
