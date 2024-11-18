using HRM.Apis.Setting;
using HRM.Repositories.Dtos.Models;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using HRM.Repositories.Setting;
using HRM.Repositories.Helper;


namespace HRM.Services.User
{
    public enum EmailType
    {

    }
    public interface IEmailService
    {
        string TemplateContent { get; }
        Task SendEmailToRecipient(Email emailRequest);
        Task SendEmailToMultipleRecipients(MultiRecipientEmail emailRequests);
    }
    public class EmailService : IEmailService
    {
        private readonly EmailSetting _serverMailSetting;
        private readonly CompanySetting _serverCompanySetting;
        private string templateContent;
        private const string FOLDER = "Email";
        private const string TEMPLATE_FILE = "Template.html";

        public string TemplateContent => templateContent;

        public EmailService(IOptions<EmailSetting> serverMailSetting,IOptions<CompanySetting> serverCompanySetting)
        {
            _serverMailSetting = serverMailSetting.Value;
            _serverCompanySetting = serverCompanySetting.Value;
            templateContent = HandleFile.READ_FILE(FOLDER, TEMPLATE_FILE)
                .Replace("{title}", _serverCompanySetting.CompanyName)
                .Replace("{currentYear}", DateTime.Now.Year.ToString());
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
