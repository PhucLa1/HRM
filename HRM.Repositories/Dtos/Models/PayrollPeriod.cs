using FluentValidation;
using HRM.Data.Entities;

namespace HRM.Repositories.Dtos.Models
{
    public class PayrollPeriod
    {
        public MonthPeriod Month { get; set; }
        public int Year { get; set; }
    }

    public class PayrollFilter
    {
        public string Dfrom { get; set; } = DateTime.Now.ToString("yyyy/MM/dd");
        public string Dto { get; set; } = DateTime.Now.ToString("yyyy/MM/dd");

        public List<int> EmployeeIds { get; set; } = new List<int>();
    }

}
