using HRM.Data.Entities;
using HRM.Repositories.Dtos.Models;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Requests
{

    public class LeaveApplicationUpsertRequestExample : IExamplesProvider<LeaveApplicationUpSert>
    {
        public LeaveApplicationUpSert GetExamples()
        {
            return new LeaveApplicationUpSert
            {
                EmployeeId = 1,
                RefuseReason = null,
                TimeLeave = 13,
                StatusLeave = StatusLeave.Draft,
                Description = "Xin nghỉ về thăm người nhà",
                ReplyMessage = null
            };
        }
    }
}
