using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class Employee : BaseEntities
    {
        public int ContractId { get; set; }
        public string? Name { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool Gender { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public int DepartmentId { get; set; } 
        public int PositionId { get; set; } 
        public DateTime DateHired { get; set; }
        public StatusEmployee StatusEmployee { get; set; } 
        public string? NationalID { get; set; }
        public string? Avatar { get; set; }
        public int UserId { get; set; }
        public Contract? Contract { get; set; }
        public ICollection<JobPosting>? jobPostings { get; set; }
        public Department? Department { get; set; } 
        public User? User { get; set; }
        public ICollection<PartimePlan>? partimePlans { get; set; }
        public ICollection<LeaveApplication>? leaveApplications { get; set; }
        public ICollection<TaxDeductionDetails>? taxDeductionDetails { get; set; }
        public ICollection<Advance>? advances { get; set; }
        public ICollection<Payroll>? payrolls { get; set; }



	}
    public enum StatusEmployee
    {
        OnLeave = 1,
        Active = 2,
        Resigned = 3
    }
}
