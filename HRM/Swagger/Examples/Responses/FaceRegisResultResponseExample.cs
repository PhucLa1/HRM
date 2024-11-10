using HRM.Repositories.Dtos.Results;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Responses
{
    public class FaceRegisResultResponseExample : IExamplesProvider<ApiResponse<List<FaceRegisResult>>>
    {
        public ApiResponse<List<FaceRegisResult>> GetExamples()
        {
            return new ApiResponse<List<FaceRegisResult>>
            {
                IsSuccess = true,
                Metadata = new List<FaceRegisResult>
                {
                    new FaceRegisResult
                    {
                        Url = "1.png",
                        Descriptor = "{'0' : '0,111', ..}",
                        StatusFaceTurn = Data.Entities.StatusFaceTurn.TurnDown
                    },
                    new FaceRegisResult
                    {
                        Url = "2.png",
                        Descriptor = "{'0' : '0,111', ..}",
                        StatusFaceTurn = Data.Entities.StatusFaceTurn.TurnRight
                    }
                }
            };
        }
    }
}
