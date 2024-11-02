using HRM.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Repositories.Dtos.Models;

public class PayrollUpsert
{
    public int Id { get; set; } = 0;
    public MonthPeriod Month { get; set; } = MonthPeriod.January;
    public int Year { get; set; }
    public int EmployeeId { get; set; } = 0;
    public int ContractId { get; set; } = 0;
    public double OtherDeduction { get; set; }
    public double OtherBonus { get; set; }
    public int FomulaId { get; set; }
    public List<int> ListBonusIds { get; set; } = new List<int>();
    public List<int> ListDeductionIds { get; set; } = new List<int>();

}

public class PayrollListUpsert
{
    public PayrollPeriod Period { get; set; }
    public List<int> EmployeeIds { get; set; } = new List<int>();

}