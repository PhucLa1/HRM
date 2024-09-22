using HRM.Repositories.Dtos.Results;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Responses
{
    public class ContractTypeResponseExample : IExamplesProvider<ApiResponse<IEnumerable<ContractTypeResult>>>
    {
        public ApiResponse<IEnumerable<ContractTypeResult>> GetExamples()
        {
            return new ApiResponse<IEnumerable<ContractTypeResult>>
            {
                Metadata = new List<ContractTypeResult>
                {
                    new ContractTypeResult
                    {
                        Id = 1,
                        Name = "Fixed-term contract"
                    },
                    new ContractTypeResult
                    {
                        Id = 2,
                        Name = "Permanent contract"
                    }
                },
                IsSuccess = true,
            };
        }
    }
}
