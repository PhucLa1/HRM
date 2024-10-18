using FluentValidation;

namespace HRM.Repositories.Dtos.Models
{
    public class FomulaUpsert
    {
        public required string Name { get; set; }
        public string? ParameterName { get; set; }
        public required string FomulaDetail { get; set; }
        public string Note { get; set; } = "";
    }
    public class FomulaUpsertValidator : AbstractValidator<FomulaUpsert>
    {
        public FomulaUpsertValidator()
        {
            RuleFor(p => p.Name.Trim()).NotEmpty().WithMessage("Fomula name must not be null");
            RuleFor(p => p.FomulaDetail).Matches(@"^(?:\(\s*(?:\[\s*[a-zA-Z_][a-zA-Z0-9_]*\s*\]|\s*[-+]?\d*\.?\d+)(?:\s*[-+*/:]\s*(?:\[\s*[a-zA-Z_][a-zA-Z0-9_]*\s*\]|\s*[-+]?\d*\.?\d+))*\s*\)|\[\s*[a-zA-Z_][a-zA-Z0-9_]*\s*\]|\s*[-+]?\d*\.?\d+)(?:\s*[-+*/:]\s*(?:\(\s*(?:\[\s*[a-zA-Z_][a-zA-Z0-9_]*\s*\]|\s*[-+]?\d*\.?\d+)(?:\s*[-+*/:]\s*(?:\[\s*[a-zA-Z_][a-zA-Z0-9_]*\s*\]|\s*[-+]?\d*\.?\d+))*\s*\)|\[\s*[a-zA-Z_][a-zA-Z0-9_]*\s*\]|\s*[-+]?\d*\.?\d+))*$").WithMessage("Biểu thức không đúng cấu trúc. FomulaDetail must take from Parameters '+', '-', '*', '%', '/'.");
        }
    }
}
