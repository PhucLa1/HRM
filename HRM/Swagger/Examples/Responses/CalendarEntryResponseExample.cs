using HRM.Data.Entities;
using HRM.Repositories.Dtos.Results;
using Swashbuckle.AspNetCore.Filters;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HRM.Apis.Swagger.Examples.Responses
{
    public class CalendarEntryResponseExample : IExamplesProvider<ApiResponse<List<CalendarEntry>>>
    {
        public ApiResponse<List<CalendarEntry>> GetExamples()
        {
            return new ApiResponse<List<CalendarEntry>>
            {
                IsSuccess = true,
                Metadata = new List<CalendarEntry>
                {
                    new CalendarEntry
                    {
                        DayOfWeek = "Sunday",
                        Date = new DateOnly(2024, 10, 29),
                        UserCalendarResult = new List<UserCalendarResult>
                        {
                            new UserCalendarResult
                            {
                                ShiftTime = ShiftTime.Morning,
                                IsCheck = true,
                                PresentShift = new DateOnly(2024, 10, 29),
                                UserCalendarStatus = UserCalendarStatus.Approved
                            },
                            new UserCalendarResult
                            {
                                ShiftTime = ShiftTime.Afternoon,
                                IsCheck = true,
                                PresentShift = new DateOnly(2024, 10, 29),
                                UserCalendarStatus = UserCalendarStatus.Approved
                            },
                        },
                        HistoryEntryResults = new Dictionary<ShiftTime, List<HistoryResult>>
                        {
                            {
                                ShiftTime.Morning,
                                new List<HistoryResult>
                                {
                                    new HistoryResult
                                    {
                                        Id = 1,
                                        StatusHistory = StatusHistory.In,
                                        TimeSweep =  new DateTime(2024,10, 29, 9,0,0),
                                        EmployeeId = 1
                                    },
                                    new HistoryResult
                                    {
                                        Id = 2,
                                        StatusHistory = StatusHistory.Out,
                                        TimeSweep =  new DateTime(2024,10, 29, 12,0,0),
                                        EmployeeId = 1
                                    },
                                }
                            },
                        }
                    },
                    new CalendarEntry
                    {
                        DayOfWeek = "Sunday",
                        Date = new DateOnly(2024, 10, 30),
                        UserCalendarResult = new List<UserCalendarResult>
                        {
                            new UserCalendarResult
                            {
                                ShiftTime = ShiftTime.Morning,
                                IsCheck = true,
                                PresentShift = new DateOnly(2024, 10, 30),
                                UserCalendarStatus = UserCalendarStatus.Approved
                            },
                            new UserCalendarResult
                            {
                                ShiftTime = ShiftTime.Afternoon,
                                IsCheck = true,
                                PresentShift = new DateOnly(2024, 10, 30),
                                UserCalendarStatus = UserCalendarStatus.Approved
                            },
                        },
                        HistoryEntryResults = new Dictionary<ShiftTime, List<HistoryResult>>
                        {
                            {
                                ShiftTime.Morning,
                                new List<HistoryResult>
                                {
                                    new HistoryResult
                                    {
                                        Id = 3,
                                        StatusHistory = StatusHistory.In,
                                        TimeSweep =  new DateTime(2024,10, 30, 9,0,0),
                                        EmployeeId = 1
                                    },
                                    new HistoryResult
                                    {
                                        Id = 4,
                                        StatusHistory = StatusHistory.Out,
                                        TimeSweep =  new DateTime(2024,10, 30, 12,0,0),
                                        EmployeeId = 1
                                    },
                                }
                            },
                        }
                    }
                }
            };
        }
    }
}
