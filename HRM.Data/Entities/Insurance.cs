using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class Insurance : BaseEntities
    {
        public required string Name { get; set; }
        public double PercentEmployee { get; set; }
        public double PercentCompany { get; set; }
        public string? ParameterName { get; set; }
    }
}
