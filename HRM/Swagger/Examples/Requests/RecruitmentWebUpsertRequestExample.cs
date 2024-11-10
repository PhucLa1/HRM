using HRM.Repositories.Dtos.Models;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Requests
{
	public class RecruitmentWebUpsertRequestExample : IExamplesProvider<RecruitmentWebUpsert>
	{
		public RecruitmentWebUpsert GetExamples()
		{
			return new RecruitmentWebUpsert
			{
				JobPostingId = 1,
				WebId = 1
			};
		}
	}
}
