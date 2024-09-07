using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class UserCalendar : BaseEntities
    {
        public int CalendarId { get; set; }
        public int PartimePlanId { get; set; }
    }
}
