using Microsoft.EntityFrameworkCore.Storage;

namespace HRM.Repositories.Dtos.Results
{
    public class ContractSalaryResult
    {
        public int Id { get; set; }
        public double BaseSalary { get; set; }
        public double BaseInsurance { get; set; }
        public int RequiredDays { get; set; }
        public int RequiredHours { get; set; }
        public double WageDaily { get; set; }
        public double WageHourly { get; set; }
        public double Factor { get; set; }
    }
}
