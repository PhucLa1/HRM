using HRM.Data.Entities;

namespace HRM.Repositories.Dtos.Results
{
    public class CalendarResult
    {
        public int Id { get; set; }
        public Day Day { get; set; }
        public TimeOnly TimeEnd { get; set; }
        public TimeOnly TimeStart { get; set; }
        public ShiftTime ShiftTime { get; set; }
    }
}
