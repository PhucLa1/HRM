namespace HRM.Repositories.Dtos.Results
{
    public class UserJwt
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public Role Role { get; set; }
    }
    public enum Role
    {
        Admin = 1,
        User = 2,
        HR = 3,
        DepartmentHead = 4
    }
    public static class RoleExtensions
    {
        public const string ADMIN_ROLE = "AdminRole";
        public const string USER_ROLE = "UserRole";
        public const string HR_ROLE = "HRRole";
        public const string DEPARTMENT_HEAD_ROLE = "DepartmentHeadRole";
    }
}
