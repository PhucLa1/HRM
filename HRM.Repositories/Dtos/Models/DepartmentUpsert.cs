using FluentValidation;

namespace HRM.Repositories.Dtos.Models
{
    public class DepartmentUpsert
    {
        public required string Name { get; set; }
        public int? ManagerId { get; set; }
    }
    public class DepartmentUpsertValidator : AbstractValidator<DepartmentUpsert>
    {
        public DepartmentUpsertValidator()
        {
            RuleFor(p => p.Name.Trim())
                .NotEmpty().WithMessage("Tên không được để trống.");
        }
    }
}
