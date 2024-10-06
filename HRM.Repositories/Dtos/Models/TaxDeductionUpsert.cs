using FluentValidation;

namespace HRM.Repositories.Dtos.Models
{
    public class TaxDeductionUpsert
    {
        public required string Name { get; set; }
        public required int FomulaType { get; set; }
        public required string ParameterName { get; set; }
        public string Terms { get; set; } = "";
    }
    public class TaxDeductionUpsertValidator : AbstractValidator<TaxDeductionUpsert>
    { 
        public TaxDeductionUpsertValidator()
        {
            RuleFor(p => p.Name.Trim()).NotEmpty().WithMessage("Amount name must not be null");
            RuleFor(p => p.FomulaType).Must(value => value == 1 || value == 2 || value == 3).WithMessage("FomulaType must be 1 or 2 or 3.");
            RuleFor(p => p.ParameterName).NotEmpty().Must(x => x.StartsWith("PARAM_TAXDEDUCTION_")).WithMessage("Parameter Name must start with 'PARAM_TAXDEDUCTION_'");
        }
    }
}
