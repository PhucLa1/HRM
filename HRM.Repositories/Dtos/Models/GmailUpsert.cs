using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Repositories.Dtos.Models
{
	public class GmailUpsert
	{
		public string? From { get; set; }
		public string? To { get; set; }
		public string? Body { get; set; }
		public DateTime MailDateTime { get; set; }
		public List<string>? Attachments { get; set; }
		public string? msgID { get; set; }
	}
	public class GmailUpsertValidator : AbstractValidator<GmailUpsert>
	{
		public GmailUpsertValidator()
		{
			RuleFor(p => p.From.Trim())
				.NotEmpty().WithMessage("Tên không được để trống.");
			RuleFor(p => p.Body.Trim())
				   .NotEmpty().WithMessage("Body không được để trống.");
		}
	}

}
