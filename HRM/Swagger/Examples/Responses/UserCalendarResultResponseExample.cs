using HRM.Repositories.Dtos.Results;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Responses
{
    public class UserCalendarResultResponseExample : IExamplesProvider<ApiResponse<List<UserCalendarResult>>>
    {
        public ApiResponse<List<UserCalendarResult>> GetExamples()
        {
            return new ApiResponse<List<UserCalendarResult>>
            {
                IsSuccess = true,
                Metadata = new List<UserCalendarResult>
                {
                    new UserCalendarResult
                    {
                        ShiftTime = Data.Entities.ShiftTime.Evening,
                        PresentShift = new DateOnly(2024, 10, 19),
                        UserCalendarStatus = Data.Entities.UserCalendarStatus.Submit
                    },
                    new UserCalendarResult
                    {
                        ShiftTime = Data.Entities.ShiftTime.Afternoon,
                        PresentShift = new DateOnly(2024, 10, 19),
                        UserCalendarStatus = Data.Entities.UserCalendarStatus.Submit
                    }
                }
            };
        }
    }
}
