using HRM.Data.Entities;
using HRM.Repositories.Dtos.Results;
using Swashbuckle.AspNetCore.Filters;


namespace HRM.Apis.Swagger.Examples.Responses
{

    public class LeaveApplicationResponseExample : IExamplesProvider<ApiResponse<IEnumerable<LeaveApplicationResult>>>
    {
        public ApiResponse<IEnumerable<LeaveApplicationResult>> GetExamples()
        {
            return new ApiResponse<IEnumerable<LeaveApplicationResult>>
            {
                Metadata = new List<LeaveApplicationResult>
                {
                    new LeaveApplicationResult
                    {
                        Id = 1,
                        EmployeeId = 1,
                        RefuseReason = null,
                        TimeLeave = 13,
                        StatusLeave = StatusLeave.Draft,
                        Description = "Xin nghỉ về thăm người nhà",
                        ReplyMessage = null
                    },
                    new LeaveApplicationResult
                    {
                        Id = 2,
                        EmployeeId = 1,
                        RefuseReason = null,
                        TimeLeave = 14,
                        StatusLeave = StatusLeave.Refuse,
                        Description = "Xin nghỉ về thăm người nhà",
                        ReplyMessage = "Không đồng ý"
                    }
                },
                IsSuccess = true,
            };
        }
    }
}
