using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace SushiConsoleDev.Email
{
    internal class EmailSender
    {
        public static async Task<bool> SendEmail(string email, string subject, string body)
        {
            var fromAddress = new MailAddress("itacademymailsender@gmail.com");
            var toAddress = new MailAddress(email);
            const string fromPassword = "securepass";

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
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
            })
            {
                await smtp.SendMailAsync(message);
            }

            return true;
        }
    }
}