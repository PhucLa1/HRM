using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class Test : BaseEntities
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<Questions>? Questions { get; set; }
    }
}
