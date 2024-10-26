using HRM.Data.Entities;

namespace HRM.Repositories.Dtos.Results
{
    public class PartimePlanResult
    {
        public int Id { get; set; }
        public DateOnly TimeStart { get; set; }
        public DateOnly TimeEnd { get; set; }
        public StatusCalendar StatusCalendar { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? EmployeeName { get; set; }
        public int DiffTime { get; set; }
    }
}
