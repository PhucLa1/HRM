using FluentValidation;
using HRM.Data.Entities;

namespace HRM.Repositories.Dtos.Models
{
    public class UserCalendarInsert
    {
        public ShiftTime ShiftTime { get; set; }
        public DateOnly PresentShift { get; set; }
    }

    public class UserCalendarInsertValidator : AbstractValidator<UserCalendarInsert>
    {
        public UserCalendarInsertValidator()
        {
            RuleFor(x => x.ShiftTime)
                .NotNull()
                .WithMessage("Không được bỏ trống ca làm việc.")
                .IsInEnum()
                .WithMessage("Ca làm việc không hợp lệ.");

            RuleFor(x => x.PresentShift)
                .NotNull()
                .WithMessage("Ngày làm việc không được bỏ trống.")
                .Must(BeAFutureDate)
                .WithMessage("Ngày làm việc phải lớn hơn hiện tại 1 ngày.");
        }

        private bool BeAFutureDate(DateOnly presentShift)
        {
            return presentShift > DateOnly.FromDateTime(DateTime.Now.AddDays(1));
        }
    }
}
