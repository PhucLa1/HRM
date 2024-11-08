using HRM.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Repositories.Dtos.Results
{
	public class JobPostingResult
	{
		public int Id { get; set; }
		public string? PositionName { get; set; }
		public int PositionId { get; set; }
		public string? Description { get; set; }
		public string? Location { get; set; }
		public int SalaryRangeMin { get; set; }
		public int SalaryRangeMax { get; set; }
		public DateTime PostingDate { get; set; }
		public DateTime ExpirationDate { get; set; }
		public string? ExperienceRequired { get; set; }
		public string? EmployeeName { get; set; }
		public int EmployeeId { get; set; }
	}
}
