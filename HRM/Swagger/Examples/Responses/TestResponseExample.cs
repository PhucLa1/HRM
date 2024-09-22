using HRM.Repositories.Dtos.Results;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Responses
{
	public class TestResponseExample : IExamplesProvider<ApiResponse<IEnumerable<TestResult>>>
	{
		public ApiResponse<IEnumerable<TestResult>> GetExamples()
		{
			return new ApiResponse<IEnumerable<TestResult>>
			{
				Metadata = new List<TestResult>
				{
					new TestResult
					{
						Id = 1,
						Name = "Programming Fundamentals",
						Description = "A test to measure understanding of basic programming concepts."
					},
					new TestResult
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
