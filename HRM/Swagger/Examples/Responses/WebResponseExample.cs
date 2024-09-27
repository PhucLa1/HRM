using HRM.Repositories.Dtos.Results;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Responses
{
    public class WebResponseExample : IExamplesProvider<ApiResponse<IEnumerable<WebResult>>>
	{
		public ApiResponse<IEnumerable<WebResult>> GetExamples()
		{
			return new ApiResponse<IEnumerable<WebResult>>
			{
				Metadata = new List<WebResult>
				{
					new WebResult
					{
						Id = 1,
						Name = "Linkedin",
						WebApi = "https://api.linkedin.com/v2/ugcPosts"
					}
				},
				IsSuccess = true,
			};
		}
	}
}
