using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Repositories.Dtos.Results
{
	public class RecruitmentWebResult
	{
		public int Id { get; set; }
		public int WebId { get; set; }
		public string? Name { get; set; }
		public int JobPostingId { get; set; }
		public string? Description { get; set; }
		public string? Location { get; set; }
		public int SalaryRangeMin { get; set; }
		public int SalaryRangeMax { get; set; }
		public DateTime PostingDate { get; set; }
		public DateTime ExpirationDate { get; set; }
		public string? ExperienceRequired { get; set; }
		
	}
}
