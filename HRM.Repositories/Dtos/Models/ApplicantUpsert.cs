using FluentValidation;
using HRM.Data.Entities;
using Microsoft.AspNetCore.Http;

namespace HRM.Repositories.Dtos.Models
{
	public class ApplicantUpsert
	{
		public required string Name { get; set; }
		public required string Email { get; set; }
		public required string Phone { get; set; }
		public IFormFile? file { get; set; }
		public int PositionId { get; set; }
		public double? Rate { get; set; }
		public int? TestId { get; set; }
		public int? InterviewerId { get; set; }
		public ApplicantStatus Status { get; set; }
	}
	public class ApplicantUpsertValidator : AbstractValidator<ApplicantUpsert>
	{
		public ApplicantUpsertValidator()
		{
			RuleFor(p => p.Name.Trim())
				.NotEmpty().WithMessage("Tên không được để trống.");
			RuleFor(p => p.Email.Trim())
				   .NotEmpty().WithMessage("Email không được để trống.");
			RuleFor(p => p.Phone.Trim())
				   .NotEmpty().WithMessage("Điện thoại không được để trống.");
		}
	}
}
