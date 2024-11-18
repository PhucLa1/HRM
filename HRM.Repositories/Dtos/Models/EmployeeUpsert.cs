using FluentValidation;

namespace HRM.Repositories.Dtos.Models
{
    public class EmployeeUpsert
    {
        public int ContractId { get; set; }
        public string? PhoneNumber { get; set; }
        public required string Email { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? Avatar { get; set; }
        public List<int> TaxDeductionIds { get; set; } = new List<int>();
    }
    public class EmployeeUpsertValidator : AbstractValidator<EmployeeUpsert>
    {
        public EmployeeUpsertValidator()
        {
            RuleFor(s => s.Email).NotEmpty().WithMessage("Email address is required").EmailAddress().WithMessage("A valid email is required");
           // RuleFor(p => p.Password) .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

        }
    }
}
