using HRM.Repositories.Dtos.Results;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Responses
{
	public class TestResponseExample : IExamplesProvider<ApiResponse<IEnumerable<TestResults>>>
	{
		public ApiResponse<IEnumerable<TestResults>> GetExamples()
		{
			return new ApiResponse<IEnumerable<TestResults>>
			{
				Metadata = new List<TestResults>
				{
					new TestResults
					{
						Id = 1,
						Name = "Programming Fundamentals",
						Description = "A test to measure understanding of basic programming concepts."
					},
					new TestResults
					{
						Id = 2,
						Name = "Logical Reasoning Test",
						Description = "This test measures logical thinking and problem-solving skills."
					}
				},
				IsSuccess = true,
			};
		}
	}
}
