using HRM.Repositories.Dtos.Results;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Responses
{
	public class TestResultResponseExample : IExamplesProvider<ApiResponse<IEnumerable<TestResultResult>>>
	{
		public ApiResponse<IEnumerable<TestResultResult>> GetExamples()
		{
			return new ApiResponse<IEnumerable<TestResultResult>>
			{
				Metadata = new List<TestResultResult>
				{
					new TestResultResult
					{
						Id = 1,
						ApplicantId = 1,
						ApplicantName = "John Doe",
						QuestionsId = 1,
						QuestionText = "What is the difference between a web designer and a web developer?",
						Point = 4.5
					}
				},
				IsSuccess = true,
			};
		}
	}
}
