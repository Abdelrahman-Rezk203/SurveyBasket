using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using SurveyBasket.API.Authentication;
using SurveyBasket.API.Settings;

namespace SurveyBasket.API.Services
{
    public class EmailService : IEmailSender
    {
        private readonly MailSettings _MailSettings; 
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<MailSettings> MailSettings,ILogger<EmailService> logger)
        {
            _MailSettings = MailSettings.Value;
            _logger = logger;
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var massage = new MimeMessage
            {
                Subject = subject, 
                Sender = MailboxAddress.Parse(_MailSettings.Mail) 
            };
            
            massage.To.Add(MailboxAddress.Parse(email));

            var builder = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };

            massage.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient(); //in package MailKit.Net.Smtp

            _logger.LogInformation("Sending Email to {email} :", email);

            smtp.Connect(_MailSettings.host, _MailSettings.port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_MailSettings.Mail,_MailSettings.Password);
            await smtp.SendAsync(massage);
            smtp.Disconnect(true);

        }
    }
}
