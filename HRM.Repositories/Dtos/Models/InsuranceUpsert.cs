using FluentValidation;

namespace HRM.Repositories.Dtos.Models
{
    public class InsuranceUpsert
    {
        public required string Name { get; set; }
        public required double PercentEmployee { get; set; }
        public required double PercentCompany { get; set; }
        public required string ParameterName { get; set; }
    }
    public class InsuranceUpsertValidator : AbstractValidator<InsuranceUpsert>
    {
        public InsuranceUpsertValidator()
        {
            RuleFor(p => p.Name.Trim())
                .NotEmpty().WithMessage("Tên không được để trống.");
        }
    }
}
