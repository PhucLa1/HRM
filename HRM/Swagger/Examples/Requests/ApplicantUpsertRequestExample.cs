using HRM.Repositories.Dtos.Models;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Requests
{
	public class ApplicantUpsertRequestExample : IExamplesProvider<ApplicantUpsert>
	{
		public ApplicantUpsert GetExamples()
		{
			return new ApplicantUpsert
			{
				Name = "John Doe",
				Email = "somebody1@gmail.com",
				Phone = "0123456789",
				FileDataStore = "C:\\Users\\ADMIN\\HRM\\HRM\\wwwroot\\CV\\Intern_CV_Full_Stack_Developer.pdf",
				PositionId = 1,
				Rate = 4.5,
				TestId = 1,
				InterviewerName = "Jane Doe" 
			};
		}
	}
}
