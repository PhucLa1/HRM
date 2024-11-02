using HRM.Repositories.Dtos.Results;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Responses
{
	public class RecruitmentWebResponseExample : IExamplesProvider<ApiResponse<IEnumerable<RecruitmentWebResult>>>
	{
		public ApiResponse<IEnumerable<RecruitmentWebResult>> GetExamples()
		{
			return new ApiResponse<IEnumerable<RecruitmentWebResult>>
			{
				Metadata = new List<RecruitmentWebResult>
				{
					new RecruitmentWebResult
					{
						Id = 1,
						JobPostingId = 1,
						WebId = 1,
						Name = "Web 1",
						Description = "Description 1",
						Location = "Location 1",
						SalaryRangeMin = 4000,
						SalaryRangeMax = 8000,
						PostingDate = DateTime.Now,
						ExpirationDate = DateTime.Now.AddMonths(1),
						ExperienceRequired = "3+ years of experience in .NET development"
					},
					new RecruitmentWebResult
					{
						Id = 2,
						JobPostingId = 2,
						WebId = 2,
						Name = "Web 2",
						Description = "Description 2",
						Location = "Location 2",
						SalaryRangeMin = 4000,
						SalaryRangeMax = 8000,
						PostingDate = DateTime.Now,
						ExpirationDate = DateTime.Now.AddMonths(1),
						ExperienceRequired = "No experiment"
					}
				},
				IsSuccess = true,
			};
		}
	}
}
