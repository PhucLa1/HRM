using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Repositories.Dtos.Models
{
	public class QuestionUpsert
	{
		public required int TestId { get; set; }
		public required string QuestionText { get; set; }
		public required double Point { get; set; }
	}
	public class QuestionUpsertValidator : AbstractValidator<QuestionUpsert>
	{
		public QuestionUpsertValidator()
		{
			/*RuleFor(p => p.QuestionText.Trim())
				.NotEmpty().WithMessage("TestName không được để trống.");*/
			RuleFor(p => p.QuestionText.Trim())
				.NotEmpty().WithMessage("Câu hỏi không được để trống.");
			RuleFor(p => p.QuestionText.Trim())
				.NotEmpty().WithMessage("Điểm không được để trống.");
		}
	}
}
