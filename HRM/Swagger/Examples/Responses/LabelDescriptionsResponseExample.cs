using HRM.Repositories.Dtos.Results;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Responses
{
    public class LabelDescriptionsResponseExample : IExamplesProvider<ApiResponse<List<LabelDescriptions>>>
    {
        public ApiResponse<List<LabelDescriptions>> GetExamples()
        {
            return new ApiResponse<List<LabelDescriptions>>
            {
                IsSuccess = true,
                Metadata = new List<LabelDescriptions>
                {
                    new LabelDescriptions
                    {
                        Name = "Label A",
                        Id = 1,
                        Descriptions = new List<string>
                        {
                            "{'0' : '0,111', ..}",
                            "{'0' : '0,111', ..}"
                        }
                    },
                    new LabelDescriptions
                    {
                        Name = "Label B",
                        Id = 2,
                        Descriptions = new List<string>
                        {
                            "{'0' : '0,111', ..}",
                            "{'0' : '0,111', ..}"
                        }
                    }
                }
            };
        }
    }
}
