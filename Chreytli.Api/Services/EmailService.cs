using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using System.Configuration;
using System.Net;

namespace Chreytli.Api.Services
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await ConfigSendGridAsync(message);
        }

        private async Task ConfigSendGridAsync(IdentityMessage message)
        {
            var mail = new MailMessage();
            mail.To.Add(message.Destination);
            mail.From = new MailAddress(ConfigurationManager.AppSettings["MailAccount"]);
            mail.Subject = message.Subject;
            mail.Body = message.Body;
            mail.IsBodyHtml = true;

            var smtp = new SmtpClient("chreyt.li");
            smtp.EnableSsl = false;
            smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["MailAccount"], ConfigurationManager.AppSettings["MailPassword"]);

            smtp.Send(mail);
        }
    }
}