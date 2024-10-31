using HRM.Data.Entities.Base;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.Data.Entities
{
    public class Applicants : BaseEntities
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? FileDataUrl { get; set; }
        public int PositionId { get; set; }
        public double Rate { get; set; }
        public int TestId { get; set; }
        public string? InterviewerName { get; set; }
        public Position? Position { get; set; }
        public Test? Test { get; set; }
        public ICollection<TestResult>? testResults { get; set; }
        [NotMapped]
		public IFormFile? file { get; set; }
	}
}
