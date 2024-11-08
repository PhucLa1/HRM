namespace HRM.Repositories.Dtos.Results
{
    public class EmployeeAttendanceRecord
    {
        public string? DayOfWeek { get; set; }
        public DateOnly Date { get; set; }
        public List<HistoryResult>? HistoryResults { get; set; }
    }
}
