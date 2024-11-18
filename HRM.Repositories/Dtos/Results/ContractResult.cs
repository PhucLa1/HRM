using HRM.Data.Entities;

namespace HRM.Repositories.Dtos.Results
{
    public class ContractResult
    {
        public double Id { get; set; }
        //Contract Salary
        public int ContractSalaryId { get; set; }
        public double BaseSalary { get; set; }
        public double BaseInsurance { get; set; }
        public int RequiredDays { get; set; }
        public double WageDaily { get; set; }
        public int RequiredHours { get; set; }
        public double WageHourly { get; set; }
        public double Factor { get; set; }
        //Contract Type
        public int ContractTypeId { get; set; }
        public string? ContractTypeName { get; set; }        
        //Department
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        //Position
        public int PositionId { get; set; }
        public string? PositionName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string FileURL { get; set; }
        public EmployeeSignStatus EmployeeSignStatus { get; set; }
        public CompanySignStatus CompanySignStatus { get; set; }
        public ContractStatus ContractStatus { get; set; }
        public TypeContract TypeContract {  get; set; }
        public string? Name { get; set; } //2  Employee name
        public DateTime DateOfBirth { get; set; }//2
        public bool Gender { get; set; }//2
        public string? Address { get; set; }//2
        public string? CountrySide { get; set; }//2
        public string? NationalID { get; set; }//2
        public DateTime NationalStartDate { get; set; }//2
        public string? NationalAddress { get; set; }//2
        public string? Level { get; set; }//2
        public string? Major { get; set; }//2
        public List<AllowanceResult>? AllowanceResults { get; set; }
        public List<InsuranceResult>? InsuranceResults { get; set; }

        public string? FireUrlBase { get; set; } //2
        public string? FileUrlSigned { get; set; } //3  

    }
}
