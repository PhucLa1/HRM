using HRM.Data.Entities;

namespace HRM.Repositories.Dtos.Results
{
    public class AccountInfo
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public Role Role { get; set; }
    }
}
