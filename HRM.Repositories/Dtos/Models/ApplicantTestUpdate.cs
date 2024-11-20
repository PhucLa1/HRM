using FluentValidation;
using HRM.Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Repositories.Dtos.Models
{
	public class ApplicantTestUpdate
	{
		public int? TestId { get; set; }
	}
	public class ApplicantTestUpdateValidator : AbstractValidator<ApplicantTestUpdate>
	{
		public ApplicantTestUpdateValidator()
		{
			RuleFor(p => p.TestId)
				.NotEmpty().WithMessage("Tên không được để trống.");
		}
	}
}
