using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class Department : BaseEntities
    {
        public required string Name { get; set; }
        public int ManagerId { get; set; }
    }
}
