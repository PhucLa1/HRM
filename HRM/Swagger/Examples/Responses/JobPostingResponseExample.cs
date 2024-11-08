using HRM.Repositories.Dtos.Results;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Responses
{
	public class JobPostingResponseExample : IExamplesProvider<ApiResponse<IEnumerable<JobPostingResult>>>
	{
		public ApiResponse<IEnumerable<JobPostingResult>> GetExamples()
		{
			return new ApiResponse<IEnumerable<JobPostingResult>>
			{
				Metadata = new List<JobPostingResult>
				{
					new JobPostingResult
					{
						Id = 1,
						PositionName = ".NET Developer",
						PositionId = 1,
						Description = "Develop and maintain web applications using ASP.NET Core.",
						Location = "Đại Mỗ",
						SalaryRangeMin = 4000,
						SalaryRangeMax = 8000,
						PostingDate = DateTime.Now,
						ExpirationDate = DateTime.Now.AddMonths(1),
						ExperienceRequired = "3+ years of experience in .NET development",
						EmployeeName = "Nguyễn Văn A",
						EmployeeId = 1
					},
					new JobPostingResult
					{
						Id = 2,
						PositionName = ".NET Developer",
						PositionId = 1,
						Description = "Cộng tác viên .NET",
						Location = "Đại Mỗ",
						SalaryRangeMin = 4000,
						SalaryRangeMax = 8000,
						PostingDate = DateTime.Now,
						ExpirationDate = DateTime.Now.AddMonths(1),
						ExperienceRequired = "No experiment",
						EmployeeName = "Nguyễn Văn B",
						EmployeeId = 1
					}
				},
				IsSuccess = true,
			};
		}
	}
}
