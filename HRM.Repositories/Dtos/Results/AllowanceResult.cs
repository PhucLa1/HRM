namespace HRM.Repositories.Dtos.Results
{
    public class AllowanceResult
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double Amount { get; set; }
        public string? Terms { get; set; }
        public string? ParameterName { get; set; }
    }
}
