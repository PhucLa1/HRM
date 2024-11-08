using HRM.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Repositories.Dtos.Results;

public class PayrollResult
{
    public int Id { get; set; } = 0;
    public MonthPeriod Month { get; set; } = MonthPeriod.January;
    public int Year { get; set; }
    public int EmployeeId { get; set; } = 0;
    public int ContractId { get; set; } = 0;
    public string EmployeeName { get; set; } = "";
    public string PositionName { get; set; } = "";
    public string DepartmentName { get; set; } = "";
    public double OtherDeduction { get; set; }
    public double OtherBonus { get; set; }
    public int FomulaId { get; set; }
    public List<int> ListBonusIds { get; set; } = new List<int>();
    public List<int> ListDeductionIds { get; set; } = new List<int>();

    //ko sửa
    public ContractSalaryResult ContractSalary { get; set; }
    public List<AllowanceResult> ListAllowance { get; set; } = new List<AllowanceResult>();
    public List<InsuranceResult> ListInsurance { get; set; } = new List<InsuranceResult>();
    public List<TaxDeductionResult> ListTaxDeduction { get; set; } = new List<TaxDeductionResult>();

}
