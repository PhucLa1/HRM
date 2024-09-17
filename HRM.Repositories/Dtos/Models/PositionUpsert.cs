using FluentValidation;

namespace HRM.Repositories.Dtos.Models
{
    public class PositionUpsert
    {
        public required string Name { get; set; }
    }
    public class PositionUpsertValidator : AbstractValidator<PositionUpsert>
    {
        public PositionUpsertValidator()
        {
            RuleFor(p => p.Name.Trim())
                .NotEmpty().WithMessage("Tên không được để trống.");
        }
    }
}
