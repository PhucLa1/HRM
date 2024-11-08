using HRM.Repositories.Dtos.Models;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Requests
{
	public class JobPostingUpsertRequestExample : IExamplesProvider<JobPostingUpsert>
	{
		public JobPostingUpsert GetExamples()
		{
			return new JobPostingUpsert
			{
				PositionId = 1,
				Description = "Develop and maintain web applications using ASP.NET Core.",
				Location = "Đại Mỗ",
				SalaryRangeMin = 4000,
				SalaryRangeMax = 8000,
				PostingDate = DateTime.Now,
				ExpirationDate = DateTime.Now.AddMonths(1),
				ExperienceRequired = "3+ years of experience in .NET development",
				EmployeeId = 1
			};
		}
	}
}
