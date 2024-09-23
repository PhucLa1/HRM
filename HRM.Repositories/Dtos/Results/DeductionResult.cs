namespace HRM.Repositories.Dtos.Results
{
    public class DeductionResult
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public double Amount { get; set; } = 0;
        public string ParameterName { get; set; } = "";
    }
}
