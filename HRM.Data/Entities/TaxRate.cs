using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class TaxRate : BaseEntities
    {
        public string? ParameterName { get; set; }
        public double Percent { get; set; }
        public double MinTaxIncome { get; set; }
        public double MaxTaxIncome { get; set; }
        public double MinusAmount { get; set; }
        public required string Name { get; set; }
        public string Condition { get; set; }
    }

}
