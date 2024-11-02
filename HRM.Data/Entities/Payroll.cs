using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class Payroll : BaseEntities
    {
        //public string? PayPeriod { get; set; }
        public MonthPeriod Month { get; set; } = MonthPeriod.January;
        public int Year { get; set; }
        public int EmployeeId { get; set; }
        public int ContractId { get; set; }
        public double OtherDeduction { get; set; }
        public double OtherBonus { get; set; }
        public int FomulaId { get; set; }
        //public Employee? Employee { get; set; }
        public Contract? Contract { get; set; }
        public ICollection<DeductionDetails>? DeductionDetails { get; set; }
        public ICollection<BonusDetails>? bonusDetails { get; set; }
        public Fomula? Fomula { get; set; }
        public ICollection<History>? History { get; set; }
        
    }

    public enum MonthPeriod
    {
        January = 1,
        February,
        March,
        April,
        May,
        June,
        July,
        August,
        September,
        October,
        November,
        December
    }
}
