using HRM.Data.Entities;
using HRM.Repositories.Dtos.Results;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Responses
{
    public class ShiftGroupResultResponseExample : IExamplesProvider<ApiResponse<List<ShiftGroup>>>
    {
        public ApiResponse<List<ShiftGroup>> GetExamples()
        {
            return new ApiResponse<List<ShiftGroup>>
            {
                IsSuccess = true,
                Metadata = new List<ShiftGroup>
                {
                    new ShiftGroup
                    {
                        ShiftTime = ShiftTime.Morning,
                        CalendarResult = new List<CalendarResult>
                        {
                            new CalendarResult
                            {
                                Day = Day.Monday,
                                ShiftTime = ShiftTime.Morning,
                                TimeEnd = new TimeOnly(12, 0),
                                TimeStart = new TimeOnly(8, 0),
                            },
                            new CalendarResult
                            {
                                Day = Day.Tuesday,
                                ShiftTime = ShiftTime.Morning,
                                TimeEnd = new TimeOnly(12, 0),
                                TimeStart = new TimeOnly(8, 0),
                            }
                        }
                    }              
                }
            };
        }
    }
}
