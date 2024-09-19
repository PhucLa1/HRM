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
        User = 2
    }
}
