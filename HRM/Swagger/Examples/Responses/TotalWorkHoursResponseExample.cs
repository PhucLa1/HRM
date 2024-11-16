using HRM.Repositories.Dtos.Results;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Responses
{
    public class TotalWorkHoursResponseExample : IExamplesProvider<ApiResponse<List<TotalWorkHours>>>
    {
        public ApiResponse<List<TotalWorkHours>> GetExamples()
        {
            return new ApiResponse<List<TotalWorkHours>>
            {
                IsSuccess = true,
                Metadata = new List<TotalWorkHours>
                {
                    new TotalWorkHours
                    {
                        EmployeeId = 1,
                        TotalWorkedHours = 48.0
                    },
                    new TotalWorkHours
                    {
                        EmployeeId = 2,
                        TotalWorkedHours = 49.0
                    }
                }
            };
        }
    }
}
