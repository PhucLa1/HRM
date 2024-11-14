using HRM.Data.Entities;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace HRM.Repositories.Dtos.Results
{

    public class TreeNode
    {
        public string Key { get; set; } = "";
        public string Label { get; set; } = "";
        public string Data { get; set; } = "";
        public string Icon { get; set; } = "";
        public List<TreeNode> Children { get; set; } = new List<TreeNode>();

    }

    public class DynamicColumn
    {
        public int Id { get; set; } = 0;
        public string Field { get; set; } = "";
        public string Header { get; set; } = "";
        public double Amount { get; set; } = 0;
        public List<int> ListIdBelongTo { get; set; } = new List<int>();
    }

    
    public class RowTableHeader
    {
        public List<ColumnTableHeader> ListColumns { get; set; } = new List<ColumnTableHeader>();
    }

    public class ColumnTableHeader: DynamicColumn
    {
        public List<int> ListParentIds { get; set; } = new List<int>();
        public int RowSpan { get; set; } = 1;
        public int ColSpan { get; set; } = 1;
    }

    public class PayrollTableData
    {
        public int PayrollId { get; set; } = 0;
        public int EmployeeId { get; set; } = 0;
        public int ContractId { get; set; } = 0;
        public string EmployeeName { get; set; } = "";
        public string PositionName { get; set; } = "";
        public string DepartmentName { get; set; } = "";
        public string Email { get; set; } = "";
        public string PhoneNumber { get; set; } = "";
        public string DateHired { get; set; } = "";


        [JsonPropertyName("dp")]
        public Dictionary<string, double> DynamicProperties { get; set; } = new Dictionary<string, double>();
    }

    public enum ColId
    {
        ColIdEmployeeName = 11,
        ColIdDepartmentName,
        ColIdBaseSalary,
        ColIdSCIncome,
        ColIdSCDeduction,
        ColIdSCTax,
        ColIdSCNotax,
        ColIdSCNet,

        ColIdHourWage,
        ColIdAllAllowance,
        ColIdAllBonus,
        ColIdTotalIncome,
        ColIdAllInsurance,
        ColIdAllTaxDeduction,
        ColIdTotalDeduction,
        ColIdNotInTax,
        ColIdTaxIncome,
        ColIdAccessableIncome,
        ColIdTaxrate,
        ColIdTaxCheckout,
        ColIdAdvance,
        ColIdAllDeductionNoTax,
        ColIdAllBonusNoTax,

    }


}
