namespace HRM.Repositories.Dtos.Results
{
    public class InsuranceResult
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double PercentEmployee { get; set; }
        public double PercentCompany { get; set; }
        public string? ParameterName { get; set; }
    }
}
