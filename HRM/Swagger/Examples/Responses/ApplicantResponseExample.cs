using HRM.Data.Entities;
using HRM.Repositories.Dtos.Results;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Responses
{
	public class ApplicantResponseExample : IExamplesProvider<ApiResponse<IEnumerable<ApplicantResult>>>
	{
		public ApiResponse<IEnumerable<ApplicantResult>> GetExamples()
		{
			return new ApiResponse<IEnumerable<ApplicantResult>>
			{
				Metadata = new List<ApplicantResult>
				{
					new ApplicantResult
					{
						Id = 1,
						Name = "John Doe",
						Email = "Kagami@gmail.com",
						Phone = "0123456789",
						FileDataStore = "C:\\Users\\ADMIN\\HRM\\HRM\\wwwroot\\CV\\Intern_CV_Full_Stack_Developer.pdf",
						PositionId = 1,
						PositionName = "Full Stack Developer",
						Rate = 4.5,
						TestId = 1,
						TestName = "Full Stack Developer Test",
						InterviewerId = 1,
						InterviewerName = "Jane Doe",
						Status = ApplicantStatus.Wait,
					}
				},
				IsSuccess = true,
			};
		}
	}
}
