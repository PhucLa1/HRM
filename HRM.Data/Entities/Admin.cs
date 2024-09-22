using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class Admin : BaseEntities
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
