namespace HRM.Repositories.Dtos.Results
{
    public class PositionResult
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int CurrentPositionsFilled { get; set; }
        public int TotalPositionsNeeded { get; set; }
        public string? DepartmentName { get; set; }
        public int DepartmentId { get; set; }
    }
}
