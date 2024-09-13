using FluentValidation;

namespace HRM.Repositories.Dtos.Models
{
    public class PositionAdd
    {
        public required string Name { get; set; }
    }
    public class PositionAddValidator : AbstractValidator<PositionAdd>
    {
        public PositionAddValidator()
        {
            RuleFor(p => p.Name.Trim())
                .NotEmpty().WithMessage("Tên không được để trống.");
        }
    }
}
