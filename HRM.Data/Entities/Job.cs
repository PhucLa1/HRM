using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class Job : BaseEntities
    {
        public required string Name { get; set; }
        public ICollection<JobPosting>? Postings { get; set; }
        
    }
}
