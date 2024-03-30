using Core.Dto.Email;
using Core.Helpers;
using Interfaces.IMailServices;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Services.MailServices
{
    public class MailServices : IMailServices
    {
        private readonly IOptions<MailConfigurations> _MailOptions;

        public MailServices(IOptions<MailConfigurations> options)
        {
            _MailOptions = options;
        }

        public async void SendEmail(Message message)
        {
            MimeMessage EmailMessage = CreateEmailMessage(message);
            Send(EmailMessage);
        }

        private void Send(MimeMessage emailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                client.Connect(_MailOptions.Value.SmtpServer, _MailOptions.Value.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_MailOptions.Value.UserName, _MailOptions.Value.Password);
                client.Send(emailMessage);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var EmailMessage = new MimeMessage();
            EmailMessage.From.Add(new MailboxAddress("email", _MailOptions.Value.From));
            EmailMessage.To.AddRange(message.To);
            EmailMessage.Subject = message.Subject;
            EmailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            return EmailMessage;
        }
    }
}
