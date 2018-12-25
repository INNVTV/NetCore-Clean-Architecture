using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Services.Email
{
    public class SendGridEmailService : IEmailService
    {
        public Settings Settings { get; set; }

        public SendGridEmailService(IConfiguration configuration)
        {
            Settings = new Settings();

            Settings.Key = configuration
                .GetSection("Email")
                .GetSection("SendGrid")
                .GetSection("Key").Value;

            Settings.FromEmail = configuration
                .GetSection("Email")
                .GetSection("FromEmail").Value;

            Settings.FromName = configuration
                .GetSection("Email")
                .GetSection("FromName").Value;
        }

        public async Task<bool> SendEmail(IEmailMessage emailMessage)
        {
            var client = new SendGridClient(Settings.Key);  

            var subject = emailMessage.Subject;
            var to = new EmailAddress(emailMessage.ToEmail, emailMessage.ToName);
            var plainTextContent = emailMessage.TextContent;
            var htmlContent = emailMessage.HtmlContent;

            var from = new EmailAddress();

            #region If not overriden by the IEmailMessage: use default 'From' settings

            if(String.IsNullOrEmpty(emailMessage.FromEmail))
            {
                from.Email = Settings.FromEmail;
            }
            else
            {
                from.Email = emailMessage.FromEmail;
            }

            if (String.IsNullOrEmpty(emailMessage.FromName))
            {
                from.Name = Settings.FromName;
            }
            else
            {
                from.Name = emailMessage.FromName;
            }

            #endregion

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            if(response.StatusCode == System.Net.HttpStatusCode.Accepted || response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
