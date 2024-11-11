namespace HRM.Repositories.Dtos.Results
{
    public class ProfileDetail
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string?  Email { get; set; }
        public string? TypeContract { get; set; }
        public string? DepartmentName { get; set; }
        public string? PositionName { get; set; }
        public string? ContractTypeName { get; set; }
        public string? Name { get; set; }
        public DateTime DOB { get; set; }
        public string? Address { get; set; }
        public string? Gender { get; set; }
        public string? Countryside { get; set; }
        public string? NationalId { get; set; }
        public string? Level { get; set; }
        public string? Major { get; set; }
        public double BaseSalary { get; set; }
        public double BaseInsurance { get; set; }
        public int RequiredDays { get; set; }
        public int RequiredHours { get; set; }
        public double WageDaily { get; set; }
        public double WageHourly { get; set; }
        public double Factor { get; set; }

        public string? FireUrlBase { get; set; }
        public string? FileUrlSigned { get; set; }
    }
}
