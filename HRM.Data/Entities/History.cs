using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class History : BaseEntities
    {
        public StatusHistory StatusHistory { get; set; }
        public DateTime TimeSweep { get; set; }
        public int PayrollId { get; set; }
        public Payroll? Payroll { get; set; }
    }
    public enum StatusHistory
    {
        In = 1,
        Out = 2
    }
}
