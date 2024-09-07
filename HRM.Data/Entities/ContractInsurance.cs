using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class ContractInsurance : BaseEntities
    {
        public int ContractId { get; set; }
        public int InsuranceId { get; set; }
    }
}
