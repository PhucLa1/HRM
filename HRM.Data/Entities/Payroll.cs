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
    }
}
