using HRM.Repositories.Dtos.Models;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Requests
{
	public class WebUpsertRequestExample : IExamplesProvider<WebUpsert>
	{
		public WebUpsert GetExamples()
		{
			return new WebUpsert { 
				
				Name = "Linkedin",
				webApi = "https://api.linkedin.com/v2/ugcPosts"
			};
		}
	}
}
