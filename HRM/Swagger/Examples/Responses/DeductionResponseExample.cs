using HRM.Repositories.Dtos.Results;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Responses
{
    public class DeductionResponseExample : IExamplesProvider<ApiResponse<IEnumerable<DeductionResult>>>
    {
        public ApiResponse<IEnumerable<DeductionResult>> GetExamples()
        {
            return new ApiResponse<IEnumerable<DeductionResult>>
            {
                Metadata = new List<DeductionResult>
                {
                    new DeductionResult
                    {
                        Id = 1,
                        Name = "Đi làm muộn",
                        Amount = 100000,
                        ParameterName = "PARAM_DEDUCTION_LATE"
                    },
                    new DeductionResult
                    {
                        Id = 2,
                        Name = "Vi phạm nội quy 3",
                        Amount = 150000,
                        ParameterName = "PARAM_DEDUCTION_RULE3"
                    }
                },
                IsSuccess = true,
            };
        }
    }
}
