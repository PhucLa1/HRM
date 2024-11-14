using FluentValidation;

namespace HRM.Repositories.Dtos.Models
{
    public class TaxDeductionUpsert
    {
        public required string Name { get; set; }
        public required double Amount { get; set; }
        public required string ParameterName { get; set; }
        public string Terms { get; set; } = "";
    }
    public class TaxDeductionUpsertValidator : AbstractValidator<TaxDeductionUpsert>
    { 
        public TaxDeductionUpsertValidator()
        {
            RuleFor(p => p.Name.Trim()).NotEmpty().WithMessage("Amount name must not be null");
            RuleFor(p => p.Amount).Must(value => value > 0).WithMessage("Amount must be > 0.");
            RuleFor(p => p.ParameterName).NotEmpty().Must(x => x.StartsWith("PARAM_TAXDEDUCTION_")).WithMessage("Parameter Name must start with 'PARAM_TAXDEDUCTION_'");
            RuleFor(p => p.ParameterName).NotEmpty().Must(x => !x.Contains("-") && !x.Contains("+") && !x.Contains("+") && !x.Contains("/") && !x.Contains("*") && !x.Contains("%")).WithMessage("Parameter Name must not contains '+/-/*/%/'");

        }
    }
}
