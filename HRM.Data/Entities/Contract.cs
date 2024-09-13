using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class Contract : BaseEntities
    {
        public int ContractSalaryId { get; set; }
        public int ContractTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime SignDate { get; set; }
        public string? FileUrl { get; set; }
        public EmployeeSignStatus EmployeeSignStatus { get; set; }
        public CompanySignStatus CompanySignStatus { get; set; }
        public ContractStatus ContractStatus { get; set; }
        public TypeContract TypeContract { get; set; }
        public ContractSalary? ContractSalary { get; set; }
        public ICollection<ContractAllowance>? ContractAllowances { get; set; }
        public ICollection<ContractInsurance>? ContractInsurances { get; set; }
        
	}
    public enum EmployeeSignStatus
    {
        Signed = 1, 
        NotSigned = 2
    }
    public enum CompanySignStatus
    {
        Signed = 1,
        NotSigned = 2
    }
    public enum ContractStatus
    {
        Expired = 1,
        Valid = 2,
        Canceled = 3
    }
    public enum TypeContract
    {
        Partime = 1,
        Fulltime = 2
    }
}
