using FluentValidation;

namespace HRM.Repositories.Dtos.Models
{
    public class ContractTypeUpsert
    {
        public required string Name { get; set; }
    }
    public class ContractTypeUpsertValidator : AbstractValidator<ContractTypeUpsert>
    {
        public ContractTypeUpsertValidator()
        {
            RuleFor(p => p.Name.Trim())
                .NotEmpty().WithMessage("Tên loại hợp đồng không được để trống.");
        }
    }
}
