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
            RuleFor(p => p.ParameterName).NotEmpty().Must(x => x.StartsWith("PARAM_ALLOWANCE_")).WithMessage("Parameter Name must start with 'PARAM_ALLOWANCE_''");
            RuleFor(p => p.ParameterName).NotEmpty().Must(x => !x.Contains("-") && !x.Contains("+") && !x.Contains("+") && !x.Contains("/") && !x.Contains("*") && !x.Contains("%")).WithMessage("Parameter Name must not contains '+/-/*/%/'");

        }
    }
}
