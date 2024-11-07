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
		public string? Comment { get; set; }
	}
	public class TestResultUpsertValidator : AbstractValidator<TestResultUpsert>
	{
		public TestResultUpsertValidator()
		{
			RuleFor(p => p.ApplicantId)
				.NotEmpty().WithMessage("ApplicantId không được để trống.");
			RuleFor(p => p.QuestionsId)
				.NotEmpty().WithMessage("ApplicantId không được để trống.");
		}
	}
}
