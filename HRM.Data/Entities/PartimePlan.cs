using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class PartimePlan : BaseEntities
    {
        public string? Reason { get; set; }
        public StatusCalendar StatusCalendar { get; set; }
        public DateOnly TimeStart { get; set; }
        public DateOnly TimeEnd { get; set; }
        public int EmployeeId { get; set; }
        public ICollection<UserCalendar>? UserCalendars { get; set; }
    }
    public enum StatusCalendar
    {
        Draft = 1,
        Submit = 2,
        Approved = 3,
        Refuse = 4,
        Cancel = 5
    }

}
