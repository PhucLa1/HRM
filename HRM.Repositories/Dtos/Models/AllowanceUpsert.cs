using FluentValidation;

namespace HRM.Repositories.Dtos.Models
{
    public class AllowanceUpsert
    {
        public required string Name { get; set; }
        public required double Amount { get; set; }
        public required string Terms { get; set; }
        public required string ParameterName { get; set; }
    }
    public class AllowanceUpsertValidator : AbstractValidator<AllowanceUpsert>
    {
        public AllowanceUpsertValidator()
        {
            RuleFor(p => p.Name.Trim())
                .NotEmpty().WithMessage("Tên không được để trống.");
        }
    }
}
