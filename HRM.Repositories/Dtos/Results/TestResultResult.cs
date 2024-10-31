using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Repositories.Dtos.Results
{
	public class TestResultResult
	{
		public int Id { get; set; }
		public int QuestionsId { get; set; }
		public int ApplicantId { get; set; }
		public double Point { get; set; }
		public string? QuestionText { get; set; }
		public string? ApplicantName { get; set; }
	}
}
