using MailKit.Net.Smtp;
using MimeKit;
using User.Management.Service.Models;
namespace User.Management.Service.Services
{
    public class EmailService : IEmailServices
    {
        private readonly EmailConfiguration _emailConfiguration;

        public EmailService(EmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }

        public void SendEmails(Message message)
        {
            var mimeMessage = CreateEmailMessage(message);
            Send(mimeMessage);
        }

        private MimeMessage CreateEmailMessage(Message message) 
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("email", _emailConfiguration.From));
            mimeMessage.To.AddRange(message.To);
            mimeMessage.Subject = message.Subject;
            mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content};

            return mimeMessage;
        }

        private void Send(MimeMessage mimeMessage) 
        {
            using var client = new SmtpClient();
            try
            {
                client.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfiguration.UserName, _emailConfiguration.Password);

                client.Send(mimeMessage);
            }
            catch (Exception ex) 
            {
            }
        }
    }
}
