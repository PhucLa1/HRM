using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class PartimePlan : BaseEntities
    {
        public int Month { get; set; }
        public int UserId { get; set; }
        public int Week { get; set; }
        public string? Reason { get; set; }
        public StatusCalendar StatusCalendar { get; set; }
        public int EmployeeId { get; set; }
        public ICollection<UserCalendar> userCalendars { get; set; }
    }
    public enum StatusCalendar
    {
        Draft = 1,
        Approved = 2,
        Refuse = 3
    }

}
