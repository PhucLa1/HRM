namespace HRM.Repositories.Dtos.Results
{
    public class TaxRateResult
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string ParameterName { get; set; } = "";
        public double Percent { get; set; } = 0;
        public string Condition { get; set; } = "";
        public double MinTaxIncome { get; set; } = 0;
        public double MaxTaxIncome { get; set; } = 0;
        public double MinusAmount { get; set; } = 0;
    }
}
