using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class Questions : BaseEntities
    {
        public int TestId { get; set; }
        public string? QuestionText { get; set; }
        public double Point { get; set; }
        public ICollection<TestResult>? testResults { get; set; }

	}
}
