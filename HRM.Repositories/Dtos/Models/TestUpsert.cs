using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
