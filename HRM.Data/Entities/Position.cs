using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class Position : BaseEntities
    {
        public required string Name { get; set; }
        public ICollection<Employee>? Employees { get; set; }
        public ICollection<JobPosting>? jobPostings { get; set; }
    }
}
