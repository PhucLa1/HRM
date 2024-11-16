using FluentValidation;
using HRM.Data.Entities;
using Newtonsoft.Json.Linq;

namespace HRM.Repositories.Dtos.Models
{
    public class ChartUpsert
    {
        public int PageFlexibleDashboardId { get; set; }
        public string? Data { get; set; } //Stringtify
        public string? Title { get; set; }
        public string? FirstDescription { get; set; }
        public string? SecondDescription { get; set; }
        public int Size { get; set; }
        public string? PropertyName { get; set; }
        public ChartType ChartType { get; set; }
    }
    public class ChartUpsertValidator : AbstractValidator<ChartUpsert>
    {
        public ChartUpsertValidator()
        {
            RuleFor(x => x.PageFlexibleDashboardId)
            .GreaterThan(0).WithMessage("PageFlexibleDashboardId phải lớn hơn 0.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Tiêu đề là bắt buộc.")
                .MaximumLength(100).WithMessage("Tiêu đề tối đa 100 ký tự.");

            RuleFor(x => x.FirstDescription)
                .MaximumLength(250).WithMessage("Mô tả thứ nhất tối đa 250 ký tự.");

            RuleFor(x => x.SecondDescription)
                .MaximumLength(250).WithMessage("Mô tả thứ hai tối đa 250 ký tự.");

            RuleFor(x => x.Size)
                .GreaterThan(0).WithMessage("Kích thước phải lớn hơn 0.");

            RuleFor(x => x.PropertyName)
                .NotEmpty().WithMessage("Tên thuộc tính là bắt buộc.")
                .MaximumLength(50).WithMessage("Tên thuộc tính tối đa 50 ký tự.");

            RuleFor(x => x.ChartType)
                .IsInEnum().WithMessage("Loại biểu đồ không hợp lệ.");

            RuleFor(x => x.Data)
                .Must(BeValidLabelValueJson).WithMessage("Dữ liệu phải ở định dạng JSON với các trường 'label' và 'value'.");
        }
        private bool BeValidLabelValueJson(string? data)
        {
            if (string.IsNullOrEmpty(data))
                return false;

            try
            {
                var jsonArray = JArray.Parse(data);
                foreach (var item in jsonArray)
                {
                    if (item["label"] == null || item["value"] == null)
                        return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
