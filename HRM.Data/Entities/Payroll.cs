using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class Payroll : BaseEntities
    {
        public string? PayPeriod { get; set; }
        public int EmployeeId { get; set; }
        public int ContractId { get; set; }
        public double OtherDeduction { get; set; }
        public double OtherBonus { get; set; }
        public int TaxRateId { get; set; }
        public int FomulaId { get; set; }
        public Contract? Contract { get; set; }
        public ICollection<DeductionDetails>? DeductionDetails { get; set; }
        public ICollection<BonusDetails>? bonusDetails { get; set; }
        public Fomula? Fomula { get; set; }
        public ICollection<History>? History { get; set; }
        public TaxRate? TaxRate { get; set; }

        
    }
}
