using FluentValidation;

namespace HRM.Repositories.Dtos.Models
{
    public class BonusUpsert
    {
        public required string Name { get; set; }
        public required double Amount { get; set; }
        public required string ParameterName { get; set; }
    }
    public class BonusUpsertValidator : AbstractValidator<BonusUpsert>
    {
        public BonusUpsertValidator()
        {
            RuleFor(p => p.Name.Trim()).NotEmpty().WithMessage("Amount name must not be null");
            RuleFor(p => p.Amount).GreaterThanOrEqualTo(0).WithMessage("Amount must greater than 0");
            RuleFor(p => p.ParameterName).NotEmpty().Must(x => x.StartsWith("PARAM_BONUS_")).WithMessage("Parameter Name must start with 'PARAM_BONUS_''");
            RuleFor(p => p.ParameterName).NotEmpty().Must(x => !x.Contains("-")&& !x.Contains("+")&& !x.Contains("+")&& !x.Contains("/")&& !x.Contains("*")&& !x.Contains("%")).WithMessage("Parameter Name must not contains '+/-/*/%/'");
        }
    }
}
