using HRM.Repositories.Dtos.Models;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Requests
{
    public class WorkPlanInsertRequestExample : IExamplesProvider<WorkPlanInsert>
    {
        public WorkPlanInsert GetExamples()
        {
            return new WorkPlanInsert
            {
                TimeStart = new DateOnly(2024, 10, 18),
                TimeEnd = new DateOnly(2024, 10, 25),
                StatusCalendar = Data.Entities.StatusCalendar.Submit,
                DayWorks = new List<UserCalendarInsert>
                {
                    new UserCalendarInsert
                    {
                        ShiftTime = Data.Entities.ShiftTime.Afternoon,
                        PresentShift = new DateOnly(2024, 10, 18)
                    },
                    new UserCalendarInsert
                    {
                        ShiftTime = Data.Entities.ShiftTime.Afternoon,
                        PresentShift = new DateOnly(2024, 10, 19)
                    }
                }
            };
        }
    }
}
