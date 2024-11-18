using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class Position : BaseEntities
    {
        public required string Name { get; set; }
        public int CurrentPositionsFilled { get; set; }
        public int TotalPositionsNeeded { get; set; }
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
        public ICollection<JobPosting>? jobPostings { get; set; }
    }
}
