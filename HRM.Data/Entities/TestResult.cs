using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class TestResult : BaseEntities
    {
        public int QuestionsId { get; set; }
        public int ApplicantId { get; set; }
        public double Point { get; set; }
    }
}
