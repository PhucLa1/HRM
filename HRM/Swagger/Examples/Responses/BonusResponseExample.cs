using HRM.Repositories.Dtos.Results;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Responses
{
    public class BonusResponseExample : IExamplesProvider<ApiResponse<IEnumerable<BonusResult>>>
    {
        public ApiResponse<IEnumerable<BonusResult>> GetExamples()
        {
            return new ApiResponse<IEnumerable<BonusResult>>
            {
                Metadata = new List<BonusResult>
                {
                    new BonusResult
                    {
                        Id = 1,
                        Name = "Thưởng doanh số tháng 10",
                        Amount = 100000,
                        ParameterName = "PARAM_BONUS_KPI10"
                    },
                    new BonusResult
                    {
                        Id = 2,
                        Name = "Leader dự án A thành công",
                        Amount = 150000,
                        ParameterName = "PARAM_BONUS_PROJECTA"
                    }
                },
                IsSuccess = true,
            };
        }
    }
}
