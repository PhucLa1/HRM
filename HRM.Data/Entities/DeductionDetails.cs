using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class DeductionDetails : BaseEntities
    {
        public double Factor { get; set; }
        public int DeductionId { get; set; }
        public int PayrollId { get; set; }
    }
}
