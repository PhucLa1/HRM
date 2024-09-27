using HRM.Data.Entities;
using HRM.Repositories.Dtos.Models;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Requests
{
    public class CalendarUpsertRequestExample : IExamplesProvider<CalendarUpsert>
    {
        public CalendarUpsert GetExamples()
        {
            return new CalendarUpsert
            {
                Day = Day.Monday,
                ShiftTime = ShiftTime.Morning,
                TimeStart = new TimeOnly(8, 0),
                TimeEnd = new TimeOnly(12, 0)
            };
        }
    }
}
