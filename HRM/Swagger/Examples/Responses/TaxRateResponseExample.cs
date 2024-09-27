using HRM.Repositories.Dtos.Results;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Responses
{
    public class TaxRateResponseExample : IExamplesProvider<ApiResponse<IEnumerable<TaxRateResult>>>
    {
        public ApiResponse<IEnumerable<TaxRateResult>> GetExamples()
        {
            return new ApiResponse<IEnumerable<TaxRateResult>>
            {
                Metadata = new List<TaxRateResult>
                {
                    new TaxRateResult
                    {
                        Id = 1,
                        Name = "Lũy tiến 5%",
                        Percent = 0.05,
                        Condition = "Thu nhập tính thuế > 11tr",
                        ParameterName = "PARAM_TAXRATE_5"
                    },
                    new TaxRateResult
                    {
                        Id = 2,
                        Name = "Lũy tiến 10%",
                        Percent = 0.10,
                        Condition = "Thu nhập tính thuế > 20tr",
                        ParameterName = "PARAM_TAXRATE_10"
                    }
                },
                IsSuccess = true,
            };
        }
    }
}
