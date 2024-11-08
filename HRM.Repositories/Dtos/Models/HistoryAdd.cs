using FluentValidation;
using HRM.Data.Entities;

namespace HRM.Repositories.Dtos.Models
{
    public class HistoryUpsert
    {
        public StatusHistory StatusHistory { get; set; }
        public DateTime TimeSweep { get; set; }
    }
    public class HistoryUpsertValidator : AbstractValidator<HistoryUpsert>
    {
        public HistoryUpsertValidator()
        {
            RuleFor(x => x.StatusHistory)
                .NotEmpty()
                .WithMessage("Không được bỏ trống trạng thái ra, vào. ")
                .IsInEnum()
                .WithMessage("Trạng thái chỉ được có ra và vào");
            RuleFor(x => x.TimeSweep)
                .NotEmpty()
                .WithMessage("Không được để trống thời gian chấm công");
        }
    }
}
