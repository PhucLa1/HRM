using FluentValidation;
using HRM.Data.Entities;

namespace HRM.Repositories.Dtos.Models
{
    public class ContractAdd
    {
        public int ContractSalaryId { get; set; } //1
        public int ContractTypeId { get; set; } //1
        public DateTime StartDate { get; set; } //1
        public DateTime EndDate { get; set; } //1
        public TypeContract TypeContract { get; set; }//1
        public int DepartmentId { get; set; } //1
        public int PositionId { get; set; } //1
        public List<int>? AllowanceIds { get; set; } 
    }
    public class ContractAddValidator : AbstractValidator<ContractAdd>
    {
        public ContractAddValidator()
        {
            RuleFor(x => x.ContractSalaryId)
                .NotEmpty()
                .WithMessage("Hợp đồng lương không được bỏ trống .");
            RuleFor(x => x.ContractTypeId)
                .NotEmpty()
                .WithMessage("Loại hợp đồng không được bỏ trống .");
            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage("Ngày bắt đầu làm việc không được để trống .");
            RuleFor(x => x.EndDate)
                .NotEmpty()
                .WithMessage("Ngày kết thúc hợp đồng làm việc không được để trống .")
                .GreaterThan(p => p.StartDate)
                .WithMessage("Thời gian kết thúc phải lớn hơn thời gian bắt đầu.");
            RuleFor(x => x.TypeContract)
                .NotEmpty()
                .WithMessage("Hợp đồng cho loại nhân viên nào không được bỏ trống .")
                .IsInEnum()
                .WithMessage("Hợp đồng cho loại nhân viên nào không hợp lệ .");
            RuleFor(x => x.PositionId)
                .NotEmpty()
                .WithMessage("Vị trí làm việc của nhân viên không được để trống");
            RuleFor(x => x.DepartmentId)
                .NotEmpty()
                .WithMessage("Phòng ban làm việc của nhân viên không được để trống .");

        }
    }
}
