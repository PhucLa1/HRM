using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Repositories.Dtos.Models
{
	public class JobUpsert
	{
		public required string Name { get; set; }
	}
	public class JobUpsertValidator : AbstractValidator<JobUpsert>
	{
		public JobUpsertValidator()
		{
			RuleFor(p => p.Name.Trim())
				.NotEmpty().WithMessage("Tên không được để trống.");
		}
	}
}
