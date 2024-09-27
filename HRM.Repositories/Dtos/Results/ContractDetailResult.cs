namespace HRM.Repositories.Dtos.Results
{
    public class ContractDetailResult
    {
        public double BaseSalary { get; set; }
        public double BaseInsurance { get; set; }
        public int RequiredDays { get; set; }
        public double WageDaily { get; set; }
        public int RequiredHours { get; set; }
        public double WageHourly { get; set; }
        public double Factor { get; set; }
        public string? ContractTypeName { get; set; }
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
    }
}
