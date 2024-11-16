using HRM.Data.Entities;
using HRM.Repositories.Dtos.Results;
using NPOI.OpenXmlFormats.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Repositories.Dtos.Models;

public class PayrollHistoryUpsert
{
    public string Name { get; set; } = "";
    public MonthPeriod Month { get; set; }
    public int Year { get; set; } = 2024;
    public string Note { get; set; } = "";
    public string PayrollHeader { get; set; } = "";
    public string PayrollColumn { get; set; } = "";
    public string PayrollData { get; set; } = "";
}
