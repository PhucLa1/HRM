using FluentValidation;
using HRM.Data.Data;
using HRM.Data.Entities;

namespace HRM.Repositories.Dtos.Models
{

    public interface IEmployeeRepository
    {
        bool Exists(int employeeId);
    }
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HRMDbContext _context;
        public EmployeeRepository(HRMDbContext context)
        {
            _context = context;
        }
        public bool Exists(int employeeId)
        {
            return _context.Employees.Any(e => e.Id == employeeId);
        }
    }
    public class LeaveApplicationUpSert
    {
        public required int EmployeeId {  get; set; }
        public string? RefuseReason { get; set; }
        public required int TimeLeave { get; set; }
        public StatusLeave StatusLeave { get; set; }
        public string? Description { get; set; }
        public string? ReplyMessage { get; set; }
    }

    public class LeaveApplicationValidator : AbstractValidator<LeaveApplicationUpSert>
    {
        private readonly IEmployeeRepository _employeeRepository;

        public LeaveApplicationValidator(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;

            RuleFor(p => p.EmployeeId)
                .NotEmpty()
                .WithMessage("Mã nhân viên không được để trống.")
                .Must(ExistInDatabase)
                .WithMessage("Mã nhân viên không tồn tại trong cơ sở dữ liệu.");

            RuleFor(p => p.TimeLeave)
                .NotEmpty()
                .WithMessage("Ca nghỉ không được để trống");
        }

        private bool ExistInDatabase(int employeeId)
        {
            return _employeeRepository.Exists(employeeId);
        }
    }
}

/*
 RuleFor(p => p.Description)
                .NotEmpty()
                .WithMessage("Lí do xin nghỉ không được bỏ trống.")
                .MaximumLength(200)
                .WithMessage("Lí do xin nghỉ không được dài quá 200 chữ.");
            RuleFor(p => p.ReplyMessage)
                .NotEmpty()
                .WithMessage("Phản hồi không được bỏ trống.")
                .MaximumLength(200)
                .WithMessage("Phản hồi không được dài quá 200 chữ.");
            RuleFor(p => p.RefuseReason)
                .NotEmpty()
                .WithMessage("Lí do từ chối không được bỏ trống.")
                .MaximumLength(200)
                .WithMessage("Lí do từ chối không được dài quá 200 chữ.");
 */