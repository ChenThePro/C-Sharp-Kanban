using System.Net;
using System.Net.Mail;

namespace Frontend.Utils
{
    public class EmailService
    {
        private const string FROM = "max_shor@yahoo.com";
        private const string SMTP_HOST = "smtp.mail.yahoo.com";
        private const int SMTP_PORT = 587;
        private const string SMTP_PASS = "bvyx rpsb lfsl ursr";

        public bool SendResetCode(string toEmail, string code)
        {
            try
            {
                using SmtpClient client = new(SMTP_HOST, SMTP_PORT)
                {
                    Credentials = new NetworkCredential(FROM, SMTP_PASS),
                    EnableSsl = true
                };
                MailMessage mail = new(FROM, toEmail)
                {
                    Subject = "Password Reset Code",
                    Body = $"Your password reset code is: {code}, next time don't forget it you kuku!",
                    IsBodyHtml = false
                };
                client.Send(mail);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}