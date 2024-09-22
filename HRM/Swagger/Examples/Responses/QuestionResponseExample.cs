using HRM.Repositories.Dtos.Results;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Responses
{
	public class QuestionResponseExample : IExamplesProvider<ApiResponse<IEnumerable<QuestionResult>>>
	{
		public ApiResponse<IEnumerable<QuestionResult>> GetExamples()
		{
			return new ApiResponse<IEnumerable<QuestionResult>>
			{
				Metadata = new List<QuestionResult>
				{
					new QuestionResult
					{
						Id = 1,
						TestId = 1,
						QuestionText = "Tại sao ?",
						Point = 2.5
					},
					new QuestionResult
					{
						Id = 2,
						TestId = 2,
						QuestionText = "Thế nào ?",
						Point = 7.5
					}
				},
				IsSuccess = true,
			};
		}
	}
}
