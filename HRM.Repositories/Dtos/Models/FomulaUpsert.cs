using FluentValidation;

namespace HRM.Repositories.Dtos.Models
{
    public class FomulaUpsert
    {
        public required string Name { get; set; }
        public required string FomulaDetail { get; set; }
        public string Note { get; set; } = "";
    }
    public class FomulaUpsertValidator : AbstractValidator<FomulaUpsert>
    {
        public FomulaUpsertValidator()
        {
            RuleFor(p => p.Name.Trim()).NotEmpty().WithMessage("Fomula name must not be null");
            RuleFor(p => p.FomulaDetail).Matches(@"^(?=.*[\+\-\*%\/])[-A-Z0-9\+\-\*%\/_]+$").WithMessage("FomulaDetail must take from Parameters '+', '-', '*', '%', '/'.");
        }
    }
}
