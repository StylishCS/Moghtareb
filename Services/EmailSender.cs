using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace Moghtareb.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var mail = Environment.GetEnvironmentVariable("SENDER_MAIL");
            var pw = Environment.GetEnvironmentVariable("SENDER_PASS");

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, pw)
            };

            return client.SendMailAsync(from: mail, recipients: email, subject: subject, body: htmlMessage);
        }
    }
}
