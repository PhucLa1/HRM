using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class Admin : BaseEntities
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public enum Role
    {
        Admin = 1,
        Partime = 2,
        FullTime = 3
    }
    public static class RoleExtensions
    {
        public const string ADMIN_ROLE = "AdminRole";
        public const string PARTIME_ROLE = "PartimeRole";
        public const string FULLTIME_ROLE = "FulltimeRole";
    }
}
