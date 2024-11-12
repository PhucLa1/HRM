using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Repositories.Dtos.Results
{
    public class ListPayrollResult
    {
        public int PayrollId { get; set; }        
        public string EmployeeName { get; set; }  
        public string PayPeriod { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
