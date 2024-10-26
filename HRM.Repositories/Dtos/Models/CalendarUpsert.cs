using FluentValidation;
using HRM.Data.Entities;

namespace HRM.Repositories.Dtos.Models
{
    public class CalendarUpsert
    {
        public Day Day { get; set; }
        public TimeOnly TimeEnd { get; set; }
        public TimeOnly TimeStart { get; set; }
        public ShiftTime ShiftTime { get; set; }
    }
    public class CalendarUpsertValidator : AbstractValidator<CalendarUpsert>
    {
        public CalendarUpsertValidator()
        {
            RuleFor(p => p.Day)
                .NotEmpty().WithMessage("Không được để trống ngày làm việc.")
                .IsInEnum().WithMessage("Ngày làm việc phải nằm trong từ thứ 2 đến thứ 7.");

            RuleFor(p => p.TimeStart)
                .NotEmpty().WithMessage("Không được để trống thời gian bắt đầu.");

            RuleFor(p => p.TimeEnd)
                .NotEmpty().WithMessage("Không được để trống thời gian kết thúc.")
                .GreaterThan(p => p.TimeStart).WithMessage("Thời gian kết thúc phải lớn hơn thời gian bắt đầu.");

            RuleFor(p => p.ShiftTime)
                .IsInEnum().WithMessage("Ca làm việc không hợp lệ.");

            // Quy tắc cho các ca làm việc
            When(p => p.ShiftTime == ShiftTime.Morning, () =>
            {
                RuleFor(p => p.TimeStart)
                    .GreaterThanOrEqualTo(new TimeOnly(8, 0)) // 8:00 AM
                    .LessThanOrEqualTo(new TimeOnly(12, 0))   // 12:00 PM
                    .WithMessage("Ca sáng chỉ được từ 8:00 đến 12:00.");

                RuleFor(p => p.TimeEnd)
                    .GreaterThan(new TimeOnly(8, 0))          // Sau 8:00 AM
                    .LessThanOrEqualTo(new TimeOnly(12, 0))   // 12:00 PM
                    .WithMessage("Ca sáng chỉ được từ 8:00 đến 12:00.");
            });

            When(p => p.ShiftTime == ShiftTime.Afternoon, () =>
            {
                RuleFor(p => p.TimeStart)
                    .GreaterThanOrEqualTo(new TimeOnly(13, 0)) // 1:00 PM
                    .LessThanOrEqualTo(new TimeOnly(17, 30))   // 5:30 PM
                    .WithMessage("Ca chiều chỉ được từ 13:00 đến 17:30.");

                RuleFor(p => p.TimeEnd)
                    .GreaterThan(new TimeOnly(13, 0))          // Sau 1:00 PM
                    .LessThanOrEqualTo(new TimeOnly(17, 30))   // 5:30 PM
                    .WithMessage("Ca chiều chỉ được từ 13:00 đến 17:30.");
            });

            When(p => p.ShiftTime == ShiftTime.Evening, () =>
            {
                RuleFor(p => p.TimeStart)
                    .GreaterThanOrEqualTo(new TimeOnly(18, 0)) // 6:00 PM
                    .LessThanOrEqualTo(new TimeOnly(21, 0))   // 9:00 PM
                    .WithMessage("Ca tối chỉ được từ 18:00 đến 21:00.");

                RuleFor(p => p.TimeEnd)
                    .GreaterThan(new TimeOnly(18, 0))          // Sau 6:00 PM
                    .LessThanOrEqualTo(new TimeOnly(21, 0))   // 9:00 PM
                    .WithMessage("Ca tối chỉ được từ 18:00 đến 21:00.");
            });
        }
    }
}
