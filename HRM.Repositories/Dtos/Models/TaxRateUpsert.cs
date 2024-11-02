using FluentValidation;

namespace HRM.Repositories.Dtos.Models
{
    public class TaxRateUpsert
    {
        public required string Name { get; set; }
        public required double Percent { get; set; }
        public required string ParameterName { get; set; }
        public string Condition { get; set; } = "";

        public double MinTaxIncome { get; set; }
        public double MaxTaxIncome { get; set; }
        public double MinusAmount { get; set; }
    }
    public class TaxRateUpsertValidator : AbstractValidator<TaxRateUpsert>
    {
        public TaxRateUpsertValidator()
        {
            RuleFor(p => p.Name.Trim()).NotEmpty().WithMessage("Tax rate name must not be null");
            RuleFor(p => p.Percent).Must(x=>x>0&&x<1).WithMessage("Percent must be a percent between 0 and 1");
            RuleFor(p => p.ParameterName).NotEmpty().Must(x => x.StartsWith("PARAM_TAXRATE_")).WithMessage("Parameter Name must start with 'PARAM_TAXRATE_'");
            RuleFor(p => p.MinTaxIncome).Must(x => x>0).WithMessage("MinTaxIncome must be greater than 0");
            RuleFor(p => p.MaxTaxIncome).GreaterThan(x=>x.MinTaxIncome).WithMessage("MaxTaxIncome must be greater than MinTaxIncome");
        }
    }
}
