using HRM.Data.Entities;
using HRM.Repositories.Dtos.Results;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Responses
{
    public class CalendarResultResponseExample : IExamplesProvider<ApiResponse<List<CalendarResult>>>
    {
        public ApiResponse<List<CalendarResult>> GetExamples()
        {
            return new ApiResponse<List<CalendarResult>>
            {
                IsSuccess = true,
                Metadata = new List<CalendarResult>
                {
                    new CalendarResult
                    {
                        Day = Day.Monday,
                        ShiftTime = ShiftTime.Afternoon,
                        TimeEnd = new TimeOnly(12, 0),
                        TimeStart = new TimeOnly(8, 0),
                    },
                    new CalendarResult
                    {
                        Day = Day.Tuesday,
                        ShiftTime = ShiftTime.Afternoon,
                        TimeEnd = new TimeOnly(12, 0),
                        TimeStart = new TimeOnly(8, 0),
                    }
                }
            };
        }
    }
}
