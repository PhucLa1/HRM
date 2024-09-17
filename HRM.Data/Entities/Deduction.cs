using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class Deduction : BaseEntities
    {
        public required string Name { get; set; }
        public double Amount { get; set; }
        public string? ParameterName { get; set; }
        public ICollection<DeductionDetails>? deductionDetails { get; set; }

    }
}
