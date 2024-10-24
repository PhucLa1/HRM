using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class UserCalendar : BaseEntities
    {
        public ShiftTime ShiftTime { get; set; }
        public DateOnly PresentShift { get; set; }
        public int PartimePlanId { get; set; }
        public UserCalendarStatus UserCalendarStatus { get; set; }
    }
    public enum UserCalendarStatus
    {
        Submit = 1,
        Approved = 2,
        Inactive = 3,
    }
}
