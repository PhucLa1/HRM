using FluentValidation;

namespace HRM.Repositories.Dtos.Models
{
    public class TaxRateUpsert
    {
        public required string Name { get; set; }
        public required double Percent { get; set; }
        public required string ParameterName { get; set; }
        public string Condition { get; set; } = "";
    }
    public class TaxRateUpsertValidator : AbstractValidator<TaxRateUpsert>
    {
        public TaxRateUpsertValidator()
        {
            RuleFor(p => p.Name.Trim()).NotEmpty().WithMessage("Tax rate name must not be null");
            RuleFor(p => p.Percent).Must(x=>x>0&&x<1).WithMessage("Percent must be a percent between 0 and 1");
            RuleFor(p => p.ParameterName).NotEmpty().Must(x => x.StartsWith("PARAM_TAXRATE_")).WithMessage("Parameter Name must start with 'PARAM_TAXRATE_'");
        }
    }
}
