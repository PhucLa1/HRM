using HRM.Repositories.Dtos.Models;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Requests
{
	public class TestUpsertRequestExample : IExamplesProvider<TestUpsert>
	{
		public TestUpsert GetExamples()
		{
			return new TestUpsert { Name = "Test for something ", Description = "Blah Blah Blah" };
		}
	}
}
