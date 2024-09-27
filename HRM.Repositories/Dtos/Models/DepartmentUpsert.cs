using FluentValidation;

namespace HRM.Repositories.Dtos.Models
{
    public class DepartmentUpsert
    {
        public required string Name { get; set; }
        public required int ManagerId { get; set; }
    }
    public class DepartmentUpsertValidator : AbstractValidator<DepartmentUpsert>
    {
        public DepartmentUpsertValidator()
        {
            RuleFor(p => p.Name.Trim())
                .NotEmpty().WithMessage("Tên không được để trống.");
            RuleFor(p => p.ManagerId)
            .NotEmpty().WithMessage("Mã quản lý không được để trống.")
            .GreaterThan(0).WithMessage("Mã quản lý phải lớn hơn 0.");
        }
    }
}
