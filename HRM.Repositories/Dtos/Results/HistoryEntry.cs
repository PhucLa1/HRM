namespace HRM.Repositories.Dtos.Results
{
    public class HistoryEntry
    {
        public DateOnly Date { get; set; }
        public List<HistoryResult>? HistoryResult { get; set; }
    }
}
