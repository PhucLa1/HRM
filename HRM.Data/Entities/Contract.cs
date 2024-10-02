using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class Contract : BaseEntities
    {
        public int ContractSalaryId { get; set; } //1
        public int ContractTypeId { get; set; } //1
        public DateTime StartDate { get; set; } //1
        public DateTime EndDate { get; set; } //1
        public DateTime SignDate { get; set; }//3
        public string? FireUrlBase { get; set; } //2
        public string? FileUrlSigned { get; set; }    //3  
        public string? Name { get; set; } //2
        public DateTime DateOfBirth { get; set; }//2
        public bool Gender { get; set; }//2
        public string? Address { get; set; }//2
        public string? CountrySide { get; set; }//2
        public string? NationalID { get; set; }//2
        public DateTime NationalStartDate { get; set; }//2
        public string? NationalAddress { get; set; }//2
        public string? Level { get; set; }//2
        public string? Major { get; set; }//2
        public int PositionId { get; set; } //1
        public EmployeeSignStatus EmployeeSignStatus { get; set; }
        public CompanySignStatus CompanySignStatus { get; set; }
        public ContractStatus ContractStatus { get; set; }
        public TypeContract TypeContract { get; set; }//1
        public ContractSalary? ContractSalary { get; set; }
        public ICollection<ContractAllowance>? ContractAllowances { get; set; }
        public Position? Position { get; set; }
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
        Canceled = 3,
        Pending = 4
    }
    public enum TypeContract
    {
        Partime = 1,
        Fulltime = 2
    }
}
