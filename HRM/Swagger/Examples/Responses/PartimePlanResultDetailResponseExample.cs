using HRM.Repositories.Dtos.Results;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Responses
{
    public class PartimePlanResultDetailResponseExample : IExamplesProvider<ApiResponse<PartimePlanResult>>
    {
        public ApiResponse<PartimePlanResult> GetExamples()
        {
            return new ApiResponse<PartimePlanResult>
            {
                Metadata = new PartimePlanResult
                    {
                        Id = 2,
                        TimeStart = new DateOnly(2024,10,19),
                        TimeEnd = new DateOnly(2024, 10, 26),
                        StatusCalendar = Data.Entities.StatusCalendar.Approved,
                        EmployeeName = "Pham Thi Lan Anh",
                        DiffTime = 7
                    },
                IsSuccess = true,
            };
        }
    }
}
