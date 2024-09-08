using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class Bonus : BaseEntities
    {
        public string? ParameterName { get; set; }
        public double Amount { get; set; }
        public required string Name { get; set; }
        public ICollection<BonusDetails>? bonusDetails { get; set; }
    }
}
