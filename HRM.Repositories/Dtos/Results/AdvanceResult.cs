using HRM.Data.Entities;

namespace HRM.Repositories.Dtos.Results
{
    public class AdvanceResult
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public string PayPeriod { get; set; } = "";
        public string EmployeeName { get; set; } = "";
        public int EmployeeId { get; set; }
        public string Reason { get; set; } = "";
        public string Note { get; set; } = "";
        public AdvanceStatus Status { get; set; }
        public string StatusName { get; set; } = "";
    }
}
