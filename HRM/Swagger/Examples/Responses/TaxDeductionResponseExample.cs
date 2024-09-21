using HRM.Repositories.Dtos.Results;
using MathNet.Numerics.LinearAlgebra;
using NPOI.SS.Formula;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Responses
{
    public class TaxDeductionResponseExample : IExamplesProvider<ApiResponse<IEnumerable<TaxDeductionResult>>>
    {
        public ApiResponse<IEnumerable<TaxDeductionResult>> GetExamples()
        {
            return new ApiResponse<IEnumerable<TaxDeductionResult>>
            {
                Metadata = new List<TaxDeductionResult>
                {
                    new TaxDeductionResult
                    {
                        Id = 1,
                        Name = "Giảm trừ gia cảnh",
                        FomulaType = 1,
                        Terms = "Nhà có con nhỏ",
                        ParameterName = "PARAM_TAXDEDUCTION_GIACANH"
                    },
                    new TaxDeductionResult
                    {
                        Id = 2,
                        Name = "Giảm trừ các nhân đóng thuế",
                        FomulaType = 2,
                        Terms = "Cá nhân đóng thuế được giảm",
                        ParameterName = "PARAM_TAXDEDUCTION_PERSONAL"
                    }
                },
                IsSuccess = true,
            };
        }
    }
}
