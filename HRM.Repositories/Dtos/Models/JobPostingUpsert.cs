using FluentValidation;
using HRM.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Repositories.Dtos.Models
{
	public class JobPostingUpsert
	{
		public int PositionId { get; set; }
		public string? Description { get; set; }
		public string? Location { get; set; }
		public int SalaryRangeMin { get; set; }
		public int SalaryRangeMax { get; set; }
		public DateTime PostingDate { get; set; }
		public DateTime ExpirationDate { get; set; }
		public string? ExperienceRequired { get; set; }
		public int EmployeeId { get; set; }
	}
	public class JobPostingUpsertValidator : AbstractValidator<JobPostingUpsert>
	{
		public JobPostingUpsertValidator()
		{
			RuleFor(p => p.PositionId)
				.NotEmpty()
				.WithMessage("PositionId không được để trống");
			RuleFor(p => p.Description)
				.NotEmpty()
				.WithMessage("Description không được để trống");
			RuleFor(p => p.Location)
				.NotEmpty()
				.WithMessage("Location không được để trống");
			RuleFor(p => p.SalaryRangeMin)
				.NotEmpty()
				.WithMessage("SalaryRangeMin không được để trống");
			RuleFor(p => p.SalaryRangeMax)
				.NotEmpty()
				.WithMessage("SalaryRangeMax không được để trống");
			RuleFor(p => p.PostingDate)
				.NotEmpty()
				.WithMessage("PostingDate không được để trống");
			RuleFor(p => p.ExpirationDate)
				.NotEmpty()
				.WithMessage("PostingDate không được để trống");
			RuleFor(p => p.ExperienceRequired)
				.NotEmpty()
				.WithMessage("ExperienceRequired không được để trống");
			RuleFor(p => p.EmployeeId)
				.NotEmpty()
				.WithMessage("EmployeeId không được để trống");
		}
	}
}
