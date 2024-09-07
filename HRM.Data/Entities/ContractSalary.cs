using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class ContractSalary : BaseEntities
    {
        public double BaseSalary { get; set; }
        public double BaseInsurance { get; set; }
        public int RequiredDays { get; set; }
        public int RequiredHours { get; set; }
        public double WageDaily { get; set; }
        public double WageHourly { get; set; }
        public double Factor { get; set; }
    }
}
