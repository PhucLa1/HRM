using HRM.Repositories.Dtos.Results;
using MathNet.Numerics.LinearAlgebra;
using NPOI.SS.Formula;
using NPOI.SS.Formula.Functions;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Responses
{
    public class FomulaResponseExample : IExamplesProvider<ApiResponse<IEnumerable<FomulaResult>>>
    {
        public ApiResponse<IEnumerable<FomulaResult>> GetExamples()
        {
            return new ApiResponse<IEnumerable<FomulaResult>>
            {
                Metadata = new List<FomulaResult>
                {
                    new FomulaResult
                    {
                        Id = 1,
                        Name = "Công thức tính lương 2022",
                        FomulaDetail = "PARAM_BONUS_KPI10+PARAM_BONUS_PROJECTA-PARAM_DEDUCTION_LATE",
                        Note = "Công thức chính thức 2022"
                    },
                    new FomulaResult
                    {
                        Id = 2,
                        Name = "Công thức tính lương T7/2022",
                        FomulaDetail = "(PARAM_BONUS_KPI10-PARAM_DEDUCTION_LATE)*PARAM_TAXRATE_5",
                        Note = "Công thức tính lương dịch"
                    }
                },
                IsSuccess = true,
            };
        }
    }
}
