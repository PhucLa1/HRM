namespace HRM.Repositories.Dtos.Models
{
    public class MultiRecipientEmail
    {
        public required List<string> To { get; set; }
        public required string Subject { get; set; }
        public required string Body { get; set; }
    }
}
