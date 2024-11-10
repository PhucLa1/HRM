using FluentValidation;
using HRM.Data.Entities;

namespace HRM.Repositories.Dtos.Models
{
    public class PayrollPeriod
    {
        public MonthPeriod Month { get; set; }
        public int Year { get; set; }
    }
   
}
