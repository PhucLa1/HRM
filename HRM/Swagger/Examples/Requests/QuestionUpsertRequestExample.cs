using HRM.Repositories.Dtos.Models;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Requests
{
	public class QuestionUpsertRequestExample : IExamplesProvider<QuestionUpsert>
	{
		public QuestionUpsert GetExamples()
		{
			return new QuestionUpsert {TestId = 1 ,QuestionText = "Tại sao ?", Point = 2.0 };
		}
	}
}
