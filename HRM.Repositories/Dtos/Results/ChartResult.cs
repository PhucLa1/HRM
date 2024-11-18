using HRM.Data.Entities;

namespace HRM.Repositories.Dtos.Results
{
    public class ChartResult
    {
        public int PageFlexibleDashboardId { get; set; }
        public string? Data { get; set; } //Stringtify
        public string? Title { get; set; }
        public string? FirstDescription { get; set; }
        public string? SecondDescription { get; set; }
        public int Size { get; set; }
        public string? PropertyName { get; set; }
        public ChartType ChartType { get; set; }
    }
}
