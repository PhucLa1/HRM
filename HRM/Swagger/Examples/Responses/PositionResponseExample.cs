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
                        Name = "Software Engineer"
                    },
                    new PositionResult
                    {
                        Id = 2,
                        Name = "Product Manager"
                    }
                },
                IsSuccess = true,
            };
        }
    }
}
