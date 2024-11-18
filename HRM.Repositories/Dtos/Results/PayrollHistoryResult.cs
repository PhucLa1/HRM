using HRM.Data.Entities;
using System.Collections.Generic;

namespace HRM.Repositories.Dtos.Results;

public class PayrollHistoryResult
{
    public string Name { get; set; } = "";
    public MonthPeriod Month { get; set; }
    public int Year { get; set; } = 2024;
    public string Note { get; set; } = "";
    public List<List<ColumnTableHeader>> PayrollHeader { get; set; } = new List<List<ColumnTableHeader>>();
    public List<DynamicColumn> PayrollColumn { get; set; } = new List<DynamicColumn>();
    public List<PayrollTableData> PayrollData { get; set; } = new List<PayrollTableData>();
    public string CreatedAt { get; set; } = "";
}
