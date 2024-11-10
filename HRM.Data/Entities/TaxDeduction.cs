using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class TaxDeduction : BaseEntities
    {
        public required string Name { get; set; }
        public double Amount { get; set; }
        public string? Terms { get; set; }
        public string? ParameterName { get; set; }
        public ICollection<TaxDeductionDetails>? taxDeductionDetails { get; set; }

    }
}
