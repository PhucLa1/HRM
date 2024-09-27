using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class User : BaseEntities
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}

