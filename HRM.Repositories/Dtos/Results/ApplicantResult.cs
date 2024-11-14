using HRM.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Repositories.Dtos.Results
{
	public class ApplicantResult
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Email { get; set; }
		public string? Phone { get; set; }
		public string? FileDataStore { get; set; }
		public int PositionId { get; set; }
		public string? PositionName { get; set; }
		public double? Rate { get; set; }
		public int? TestId { get; set; }
		public string? TestName { get; set; }
		//public int? InterviewerId { get; set; }
		public string? InterviewerName { get; set; }
		public ApplicantStatus Status { get; set; }
	}
}
