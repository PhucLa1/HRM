using FluentValidation;

namespace HRM.Repositories.Dtos.Models
{
    public class PositionUpsert
    {
        public required string Name { get; set; }
        public int DepartmentId { get; set; }
        public int TotalPositionsNeeded { get; set; }
    }
    public class PositionUpsertValidator : AbstractValidator<PositionUpsert>
    {
        public PositionUpsertValidator()
        {
            RuleFor(p => p.Name.Trim())
                .NotEmpty().WithMessage("Tên không được để trống.");
            RuleFor(p => p.DepartmentId)
                .NotEmpty()
                .WithMessage("Id của phòng ban không được để trống");
            RuleFor(p => p.TotalPositionsNeeded)
                .NotEmpty()
                .WithMessage("Số người cần có cho vị trí này không được để trống");               
        }
    }
}
