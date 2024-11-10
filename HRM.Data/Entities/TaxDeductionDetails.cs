using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class TaxDeductionDetails : BaseEntities
    {
        public int EmployeeId { get; set; }
        public int TaxDeductionId { get; set; }
        public double Factor { get; set; }
    }
}
