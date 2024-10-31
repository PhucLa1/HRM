using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Repositories.Dtos.Models
{
    public class TestResultUpsert
    {
		public int QuestionsId { get; set; }
		public int ApplicantId { get; set; }
		public double Point { get; set; }
	}
	public class TestResultUpsertValidator : AbstractValidator<TestResultUpsert>
	{
		public TestResultUpsertValidator()
		{
			/*RuleFor(p => p.QuestionText.Trim())
				.NotEmpty().WithMessage("TestName không được để trống.");*/
		}
	}
}
