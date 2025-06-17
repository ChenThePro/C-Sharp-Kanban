using System.Net;
using System.Net.Mail;

public class EmailService
{
    private readonly string _from = "max_shor@yahoo.com";
    private readonly string _smtpHost = "smtp.mail.yahoo.com";
    private readonly int _smtpPort = 587;
    private readonly string _smtpUser = "max_shor@yahoo.com";
    private readonly string _smtpPass = "bvyx rpsb lfsl ursr";

    public bool SendResetCode(string toEmail, string code)
    {
        try
        {
            using var client = new SmtpClient(_smtpHost, _smtpPort)
            {
                Credentials = new NetworkCredential(_smtpUser, _smtpPass),
                EnableSsl = true
            };

            var mail = new MailMessage(_from, toEmail)
            {
                Subject = "Password Reset Code",
                Body = $"Your password reset code is: {code}",
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
