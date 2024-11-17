using HRM.Data.Entities;
using HRM.Repositories.Dtos.Results;
using NPOI.OpenXmlFormats.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Repositories.Dtos.Models;

public class PayrollHistoryModel
{
    public int Id { get; set; } = 0;
    public string Name { get; set; } = "";
    public MonthPeriod Month { get; set; }
    public int Year { get; set; } = 2024;
    public string Note { get; set; } = "";
    public List<List<ColumnTableHeader>> PayrollHeader { get; set; } = new List<List<ColumnTableHeader>>();
    public List<DynamicColumn> PayrollColumn { get; set; } = new List<DynamicColumn>();
    public List<PayrollTableData> PayrollData { get; set; } = new List<PayrollTableData>();
    public List<DynamicColumn> DisplayColumns { get; set; } = new List<DynamicColumn>();
    public string CreatedAt { get; set; } = "";

}
