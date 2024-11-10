using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Repositories.Dtos.Models
{
	public class RecruitmentWebUpsert
	{
		public required int JobPostingId { get; set; }
        public required int WebId { get; set; }
	}
	public class RecruitmentWebUpsertValidator : AbstractValidator<RecruitmentWebUpsert>
	{
		public RecruitmentWebUpsertValidator()
		{
			RuleFor(p => p.JobPostingId)
				.NotEmpty().WithMessage("Tên không được để trống.");
			RuleFor(p => p.WebId)
				   .NotEmpty().WithMessage("Đường dẫn Web được để trống.");
		}
	}
}
