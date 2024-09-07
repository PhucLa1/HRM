using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class TaxDeductionDetails : BaseEntities
    {
        public int EmployeeId { get; set; }
        public int TaxDeductionId { get; set; }
        public double Amount { get; set; }
        public int InUsed { get; set; }
        public DateTime ApplyAt { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}
