using HRM.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Repositories.Dtos.Results
{
    public class LeaveApplicationResult
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string? RefuseReason { get; set; }
        public int TimeLeave { get; set; }
        public StatusLeave StatusLeave { get; set; }
        public string? Description { get; set; }
        public string? ReplyMessage { get; set; }

    }
}
