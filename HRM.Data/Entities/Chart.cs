using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class Chart : BaseEntities
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


    public enum ChartType
    {
        BarChartHorizontal = 1,
        BarChartVertical = 2,
        PieChart = 3,
        RadialChart = 4
    }
}
