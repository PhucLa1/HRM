using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class Calendar : BaseEntities
    {
        public Day Day { get; set; }
        public TimeOnly TimeEnd { get; set; }
        public TimeOnly TimeStart { get; set; }
        public ShiftTime ShiftTime { get; set; }
    }
    public enum ShiftTime
    {
        Morning = 1,
        Afternoon = 2,
        Evening = 3
    }
    public enum Day
    {
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        Saturday = 6
    }
}
