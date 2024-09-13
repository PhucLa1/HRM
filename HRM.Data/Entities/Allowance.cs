using HRM.Data.Entities.Base;

namespace HRM.Data.Entities
{
    public class Allowance : BaseEntities
    {
        public required string Name { get; set; }
        public double Amount { get; set; }
        public string? Terms { get; set; }
        public string? ParameterName { get; set; }
        public ICollection<ContractAllowance>? ContractAllowances { get; set; }


    }
}
