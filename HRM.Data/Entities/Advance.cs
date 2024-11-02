using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class Advance : BaseEntities
    {
        public double Amount { get; set; }
        public MonthPeriod Month { get; set; }
        public int Year { get; set; }
        public int EmployeeId { get; set; }
        public string? Reason { get; set; }
        public string? Note { get; set; }
        public AdvanceStatus Status { get; set; }
    }

    public enum AdvanceStatus
    {
        Pending = 0,
        Approved = 1,
        Refused = 2
    }
}
