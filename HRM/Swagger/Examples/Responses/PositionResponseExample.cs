using HRM.Repositories.Dtos.Results;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Responses
{
    public class PositionResponseExample : IExamplesProvider<ApiResponse<IEnumerable<PositionResult>>>
    {
        public ApiResponse<IEnumerable<PositionResult>> GetExamples()
        {
            return new ApiResponse<IEnumerable<PositionResult>>
            {
                Metadata = new List<PositionResult>
                {
                    new PositionResult
                    {
                        Id = 1,
                        Name = "Fullstack Developer",
                        TotalPositionsNeeded = 1,
                        CurrentPositionsFilled = 1,
                        DepartmentName = "Công nghệ thông tin",
                        DepartmentId = 1,
                    },
                    new PositionResult
                    {
                        Id = 2,
                        Name = "Kĩ sư công nghệ thông tin",
                        TotalPositionsNeeded = 1,
                        CurrentPositionsFilled = 1,
                        DepartmentName = "Công nghệ thông tin",
                        DepartmentId = 1,

                    }
                },
                IsSuccess = true,
            };
        }
    }
}
