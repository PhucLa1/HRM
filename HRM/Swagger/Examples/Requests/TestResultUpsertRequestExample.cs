using HRM.Repositories.Dtos.Models;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Requests
{
	public class TestResultUpsertRequestExample : IExamplesProvider<TestResultUpsert>
	{
		public TestResultUpsert GetExamples()
		{
			return new TestResultUpsert
			{
				ApplicantId = 1,
				QuestionsId = 1,
				Point = 4.5
			};
		}
	}
}
