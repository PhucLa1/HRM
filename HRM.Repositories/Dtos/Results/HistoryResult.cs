using HRM.Data.Entities;

namespace HRM.Repositories.Dtos.Results
{
    public class HistoryResult
    {
        public int Id { get; set; }
        public StatusHistory StatusHistory { get; set; }
        public DateTime TimeSweep { get; set; }
        public int EmployeeId { get; set; }
    }
}
