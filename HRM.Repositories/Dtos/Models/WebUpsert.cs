using FluentValidation;

namespace HRM.Repositories.Dtos.Models
{
    public class WebUpsert
	{
		public required string Name { get; set; }	
		public required string webApi { get; set; }
	}
	public class WebUpsertValidator : AbstractValidator<WebUpsert>
	{
		public WebUpsertValidator()
		{
			RuleFor(p => p.Name.Trim())
				.NotEmpty().WithMessage("Tên không được để trống.");
			RuleFor(p => p.webApi.Trim())
				   .NotEmpty().WithMessage("Đường dẫn Web được để trống.");
		}
	}

}
