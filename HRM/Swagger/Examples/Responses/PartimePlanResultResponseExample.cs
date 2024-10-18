using HRM.Repositories.Dtos.Results;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Responses
{
    public class PartimePlanResultResponseExample : IExamplesProvider<ApiResponse<List<PartimePlanResult>>>
    {
        public ApiResponse<List<PartimePlanResult>> GetExamples()
        {
            return new ApiResponse<List<PartimePlanResult>> 
            {
                Metadata = new List<PartimePlanResult>
                {
                    new PartimePlanResult
                    {
                        Id = 1,
                        TimeStart = new DateOnly(2024,10,19),
                        TimeEnd = new DateOnly(2024, 10, 26),
                        StatusCalendar = Data.Entities.StatusCalendar.Submit,
                        EmployeeName = "La Hong Phuc",
                        DiffTime = 7
                    },
                    new PartimePlanResult
                    {
                        Id = 2,
                        TimeStart = new DateOnly(2024,10,19),
                        TimeEnd = new DateOnly(2024, 10, 26),
                        StatusCalendar = Data.Entities.StatusCalendar.Approved,
                        EmployeeName = "Pham Thi Lan Anh",
                        DiffTime = 7
                    }
                },
                IsSuccess = true,
            };
        }
    }
}
