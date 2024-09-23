namespace HRM.Repositories.Dtos.Results
{
    public class TaxDeductionResult
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int FomulaType { get; set; } = 0;
        public string Terms { get; set; } = "";
        public string ParameterName { get; set; } = "";
    }
}
