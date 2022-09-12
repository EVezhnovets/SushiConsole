using SushiConsoleDev.Logger;
using System.Net;
using System.Net.Mail;

namespace SushiConsoleDev.Email
{
    public class EmailSender
    {
        public static async Task<bool> SendEmail(string email, string subject, string body)
        {
            var fromAddress = new MailAddress("eugene.vezhnavets@gmail.com", "SushiStore");
            var toAddress = new MailAddress(email);
            const string fromPassword = "ykjcocrjsfznvohj";

            var smtp = new SmtpClient
            {
                UseDefaultCredentials = false,
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };
            using (var message = new MailMessage(fromAddress, toAddress){ Subject = subject, Body = body})
            {
                await smtp.SendMailAsync(message);
            }
            Logger.Logger.Info(typeof(EmailSender), nameof(SendEmail), "Call method");
            return true;
        }
    }
}