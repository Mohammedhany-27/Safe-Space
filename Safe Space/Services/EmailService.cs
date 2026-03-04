using System.Net;
using System.Net.Mail;

namespace SafeSpace.Services
{
    public class EmailService
    {
        public void SendVerificationEmail(string toEmail, string verificationLink)
        {
            var fromAddress = new MailAddress("midoalwakeel@gmail.com", "SafeSpace");
            var toAddress = new MailAddress(toEmail);

            const string fromPassword = "xwuhlgdrrwebocok";

            string body = $@"
                <h3>Verify Your Email</h3>
                <p>Click the link below:</p>
                <a href='{verificationLink}'>Verify Email</a>
            ";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = "SafeSpace Email Verification",
                Body = body,
                IsBodyHtml = true
            };

            smtp.Send(message);
        }
    }
}