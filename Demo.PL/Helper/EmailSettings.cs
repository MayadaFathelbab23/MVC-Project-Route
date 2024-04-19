using Demo.DAL.Models;
using Demo.PL.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net;
//using System.Net.Mail;

namespace Demo.PL.Helper
{
    public class EmailSettings : IEmailSettings
    {
        private readonly EmailConfigurations _emailConfig;
        public EmailSettings(IOptions<EmailConfigurations> emailConfig)
        {
            _emailConfig = emailConfig.Value;
        }
        public void SenEmail(Email email)
        {
            var emailMsg = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_emailConfig.From),
                Subject = email.Subject
            };
            emailMsg.To.Add(MailboxAddress.Parse(email.To));
            emailMsg.From.Add(new MailboxAddress (_emailConfig.DisplayName, _emailConfig.From));
            var builder = new BodyBuilder();
            builder.TextBody = email.Body;
            emailMsg.Body = builder.ToMessageBody();

            using(var smtp = new SmtpClient())
            {
                smtp.Connect(_emailConfig.Host , _emailConfig.Port , SecureSocketOptions.StartTls);
                smtp.Authenticate(_emailConfig.From, _emailConfig.Password);
                var response = smtp.Send(emailMsg);
                smtp.Disconnect(true);
            }
        }
    }
}
