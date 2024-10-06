using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class JobPosting : BaseEntities
    {
        public int PositionId { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public int SalaryRangeMin { get; set; }
        public int SalaryRangeMax { get; set; }
        public DateTime PostingDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string? ExperienceRequired { get; set; }
        public int EmployeeId { get; set; }
        public int WebId { get; set; }
        public ICollection<RecruitmentWeb>? recruitmentWebs { get; set; }
	}
}
