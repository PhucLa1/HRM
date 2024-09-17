using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class User : BaseEntities
    {
        public required string Password { get; set; }
        public int RoleId { get; set; }
        public Role? Role { get; set; }
    }
}
