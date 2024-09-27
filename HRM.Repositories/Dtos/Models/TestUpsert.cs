using FluentValidation;

namespace HRM.Repositories.Dtos.Models
{
    public class TestUpsert
	{
		public required string Name { get; set; }
		public required string Description { get; set; }
	}
	public class TestUpsertValidator : AbstractValidator<TestUpsert>
	{
		public TestUpsertValidator()
		{
			RuleFor(p => p.Name.Trim())
				.NotEmpty().WithMessage("Tên không được để trống.");

		}
	}
}
