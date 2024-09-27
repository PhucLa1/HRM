using HRM.Repositories.Dtos.Results;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Responses
{
	public class JobResponseExample : IExamplesProvider<ApiResponse<IEnumerable<JobResult>>>
	{
		public ApiResponse<IEnumerable<JobResult>> GetExamples()
		{
			return new ApiResponse<IEnumerable<JobResult>>
			{
				Metadata = new List<JobResult>
				{
					new JobResult
					{
						Id = 1,
						Name = "HR Manager"
					},
					new JobResult
					{
						Id = 2,
						Name = "Business specialist"
					}
				},
				IsSuccess = true,
			};
		}
	}
}
