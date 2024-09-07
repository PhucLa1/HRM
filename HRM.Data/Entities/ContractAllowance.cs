using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class ContractAllowance : BaseEntities
    {
        public int ContractId { get; set; }
        public int AllowanceId { get; set; }
    }
}
