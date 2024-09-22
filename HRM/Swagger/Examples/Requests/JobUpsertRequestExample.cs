using HRM.Repositories.Dtos.Models;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Requests
{
	public class JobUpsertRequestExample : IExamplesProvider<JobUpsert>
	{
		public JobUpsert GetExamples()
		{
			return new JobUpsert { Name = "HR Manager" };
		}
	}
}
