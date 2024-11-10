using HRM.Data.Entities;
using HRM.Repositories.Dtos.Results;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Responses
{
    public class EmployeeAttendanceRecordsResponseExample : IExamplesProvider<ApiResponse<List<EmployeeAttendanceRecord>>>
    {
        public ApiResponse<List<EmployeeAttendanceRecord>> GetExamples()
        {
            return new ApiResponse<List<EmployeeAttendanceRecord>>
            {
                IsSuccess = true,
                Metadata = new List<EmployeeAttendanceRecord>
                {
                    new EmployeeAttendanceRecord
                    {
                       DayOfWeek = "Sunday",
                       Date = new DateOnly(2024, 10, 30),
                       HistoryResults = new List<HistoryResult>
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
                    new EmployeeAttendanceRecord
                    {
                       DayOfWeek = "Monday",
                       Date = new DateOnly(2024, 10, 31),
                       HistoryResults = new List<HistoryResult>
                       {
                                    new HistoryResult
                                    {
                                        Id = 5,
                                        StatusHistory = StatusHistory.In,
                                        TimeSweep =  new DateTime(2024,10, 30, 9,0,0),
                                        EmployeeId = 1
                                    },
                                    new HistoryResult
                                    {
                                        Id = 6,
                                        StatusHistory = StatusHistory.Out,
                                        TimeSweep =  new DateTime(2024,10, 30, 12,0,0),
                                        EmployeeId = 1
                                    },
                                }
                    }
                }
            };
        }
    }
}