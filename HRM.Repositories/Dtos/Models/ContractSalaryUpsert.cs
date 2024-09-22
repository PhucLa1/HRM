using FluentValidation;

namespace HRM.Repositories.Dtos.Models
{
    public class ContractSalaryUpsert
    {
        public required double BaseSalary { get; set; }
        public required double BaseInsurance { get; set; }
        public required int RequiredDays { get; set; }
        public required int RequiredHours { get; set; }
        public required double WageDaily { get; set; }
        public required double WageHourly { get; set; }
        public required double Factor { get; set; }
    }
    public class ContractSalaryUpsertValidator : AbstractValidator<ContractSalaryUpsert>
    {
        public ContractSalaryUpsertValidator()
        {
            RuleFor(p => p.BaseSalary)
                .NotEmpty().WithMessage("BaseSalary không được để trống.");
            RuleFor(p => p.BaseInsurance)
                .NotEmpty().WithMessage("BaseInsurance không được để trống.");
        }
    }
}
