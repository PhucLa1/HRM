using FluentValidation;

namespace HRM.Repositories.Dtos.Models
{
    public class PositionUpdate : PositionAdd
    {
    }
    public class PositionUpdateValidator : AbstractValidator<PositionUpdate>
    {
        public PositionUpdateValidator()
        {
            RuleFor(p => p.Name.Trim())
                .NotEmpty().WithMessage("Tên không được để trống.");
        }
    }
}
