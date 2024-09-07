using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class Advance : BaseEntities
    {
        public double Amount { get; set; }
        public string? PayPeriod { get; set; }
        public int EmployeeId { get; set; }
    }
}
