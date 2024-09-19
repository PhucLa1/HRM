using HRM.Apis.Setting;
using HRM.Repositories.Dtos.Models;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;


namespace HRM.Services.User
{
    public interface IEmailService
    {
        Task SendEmailToRecipient(Email emailRequest);
        Task SendEmailToMultipleRecipients(MultiRecipientEmail emailRequests);
    }
    public class EmailService : IEmailService
    {
        private readonly EmailSetting _serverMailSetting;

        public EmailService(IOptions<EmailSetting> serverMailSetting)
        {
            _serverMailSetting = serverMailSetting.Value;
        }
        public async Task SendEmailToRecipient(Email emailRequest)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_serverMailSetting.EmailUsername));
                email.To.Add(MailboxAddress.Parse(emailRequest.To));
                email.Subject = emailRequest.Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = emailRequest.Body };

                using var smtp = new SmtpClient();
                smtp.Connect(_serverMailSetting.EmailHost, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_serverMailSetting.EmailUsername, _serverMailSetting.EmailPassword);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task SendEmailToMultipleRecipients(MultiRecipientEmail emailRequests)
        {
            try
            {
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse(_serverMailSetting.EmailUsername));
                foreach (var toRecipient in emailRequests.To)
                {
                    email.To.Add(MailboxAddress.Parse(toRecipient));
                }
                email.Subject = emailRequests.Subject;
                email.Body = new TextPart(TextFormat.Html) { Text = emailRequests.Body };

                using var smtp = new SmtpClient();
                smtp.Connect(_serverMailSetting.EmailHost, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_serverMailSetting.EmailUsername, _serverMailSetting.EmailPassword);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
