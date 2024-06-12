using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _config;


        public EmailSender(IConfiguration configuration)
        {
            _config = configuration;
        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            string fromMail = _config["AuthMessageSender:fromMail"];
            string fromPassword = _config["AuthMessageSender:fromPassword"];

            
            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = subject;
            message.To.Add(new MailAddress(email));
            message.Body = "<html><body> " + htmlMessage + " </body></html>";
            message.IsBodyHtml = true;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromMail, fromPassword)
            };

           
            smtp.Send(message);
            return Task.CompletedTask;
        }
    }
}
