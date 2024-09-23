using HRM.Repositories.Dtos.Results;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Responses
{
    public class AllowanceResponseExample : IExamplesProvider<ApiResponse<IEnumerable<AllowanceResult>>>
    {
        public ApiResponse<IEnumerable<AllowanceResult>> GetExamples()
        {
            return new ApiResponse<IEnumerable<AllowanceResult>>
            {
                Metadata = new List<AllowanceResult>
                {
                    new AllowanceResult
                    {
                        Id = 1,
                        Name = "Fuel Allowance",
                        Amount = 200000,
                        Terms = "Term for fuel",
                        ParameterName = "???"
                    },
                    new AllowanceResult
                    {
                        Id = 2,
                        Name = "Food Allowance",
                        Amount = 200000,
                        Terms = "Term for food",
                        ParameterName = "???"
                    },
                },
                IsSuccess = true,
            };
        }
    }
}
