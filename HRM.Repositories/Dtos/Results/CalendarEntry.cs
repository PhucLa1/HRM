using HRM.Data.Entities;

namespace HRM.Repositories.Dtos.Results
{
    public class CalendarEntry
    {
        public string? DayOfWeek { get; set; }
        public DateOnly Date { get; set; }
        public List<UserCalendarResult>? UserCalendarResult { get; set; }
        public Dictionary<ShiftTime, List<HistoryResult>>? HistoryEntryResults { get; set; }
    }
}
