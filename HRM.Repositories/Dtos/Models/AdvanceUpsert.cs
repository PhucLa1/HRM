using FluentValidation;
using HRM.Data.Entities;

namespace HRM.Repositories.Dtos.Models
{
    public class AdvanceUpsert
    {
        public double Amount { get; set; }
        public string? PayPeriod { get; set; }
        public required int EmployeeId { get; set; }
        public required string Reason { get; set; }
        public string? Note { get; set; }
        public AdvanceStatus Status { get; set; }
    }
    public class AdvanceUpsertValidator : AbstractValidator<AdvanceUpsert>
    {
        public AdvanceUpsertValidator()
        {
            RuleFor(p => p.Amount).GreaterThan(0).WithMessage("Reason must be greater than 0");
            RuleFor(p => p.Reason.Trim()).NotEmpty().WithMessage("Reason must not be null");
            RuleFor(p => p.EmployeeId).GreaterThan(0).WithMessage("Employee must be specific");
        }
    }
}
