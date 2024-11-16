using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class PageFlexibleDashboard : BaseEntities
    {
        public string? Title { get; set; }
        public string? Url { get; set; }

        public ICollection<Chart>? Charts { get; set; }
    }
}
