using HRM.Repositories.Dtos.Results;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Responses
{
    public class InsuranceResponseExample : IExamplesProvider<ApiResponse<IEnumerable<InsuranceResult>>>
    {
        public ApiResponse<IEnumerable<InsuranceResult>> GetExamples()
        {
            return new ApiResponse<IEnumerable<InsuranceResult>>
            {
                Metadata = new List<InsuranceResult>
                {
                    new InsuranceResult
                    {
                        Id = 1,
                        Name = "Health insurance",
                        PercentEmployee = 10,
                        PercentCompany = 20,
                        ParameterName = "???",
                    },
                    new InsuranceResult
                    {
                        Id = 2,
                        Name = "Social insurance",
                        PercentEmployee = 11,
                        PercentCompany = 21,
                        ParameterName = "???",
                    }
                },
                IsSuccess = true,
            };
        }
    }
}
