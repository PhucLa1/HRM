using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class PayrollHistory : BaseEntities
    {
        public string Name { get; set; } = "";
        public MonthPeriod Month { get; set; }
        public int Year { get; set; } = 2024;
        public string Note { get; set; } = "";
        public string PayrollHeader { get; set; } = "";
        public string PayrollColumn { get; set; } = "";
        public string PayrollData { get; set; } = "";
        public string DisplayColumns { get; set; } = "";
    }
}
