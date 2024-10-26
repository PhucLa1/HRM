using FluentValidation;
using HRM.Data.Entities;

namespace HRM.Repositories.Dtos.Models
{
    public class WorkPlanInsert
    {
        public DateOnly TimeStart { get; set; }
        public DateOnly TimeEnd { get; set; }
        public StatusCalendar StatusCalendar { get; set; }
        public required List<UserCalendarInsert> DayWorks { get; set; }
    }
    
    public class WorkPlanInsertValidator : AbstractValidator<WorkPlanInsert>
    {
        public WorkPlanInsertValidator()
        {
            RuleFor(x => x.TimeStart)
                .NotEmpty()
                .WithMessage("Không được để trống ngày bắt đầu")
                .Must(timeStart => timeStart > DateOnly.FromDateTime(DateTime.Today.AddDays(1)))
                .WithMessage("Ngày bắt đầu phải lớn hơn ngày hiện tại ít nhất 1 ngày");

            RuleFor(x => x.TimeEnd)
                .NotEmpty()
                .WithMessage("Không được để trống ngày kết thúc")
                .GreaterThan(x => x.TimeStart)
                .WithMessage("Ngày kết thúc phải lớn hơn ngày bắt đầu");
            
            RuleFor(x => x.StatusCalendar)
                .NotEmpty()
                .WithMessage("Không được để trống trạng thái lịch");

            RuleFor(x => x.DayWorks)
                .NotNull()
                .WithMessage("Không được để trống danh sách ngày làm việc")
                .Must(dayWorks => dayWorks != null && dayWorks.Count > 0)
                .WithMessage("Danh sách ngày làm việc phải có ít nhất 1 mục");
        }
    }

}
