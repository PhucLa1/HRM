using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class BonusDetails : BaseEntities
    {
        public double Factor { get; set; }
        public int BonusId { get; set; }
        public int PayrollId { get; set; }
    }
}
