using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class LeaveApplication : BaseEntities
    {
        public string? ReplyMessage { get; set; }
        public string? Description { get; set; }
        public StatusLeave StatusLeave { get; set; }
        public int TimeLeave { get; set; }
        public string? RefuseReason { get; set; }
        public int EmployeeId { get; set; }
    }
    public enum StatusLeave
    {
        Draft = 1,
        Approved = 2,
        Refuse = 3
    }
}
