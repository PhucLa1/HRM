using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Data;
using HRM.Services.User;
using HRM.Repositories.Helper;
using NPOI.HSSF.Record.Chart;
using Newtonsoft.Json;
using HRM.Services.TimeKeeping;

namespace HRM.Services.Salary;

public interface IPayrollsService
{
    Task<string> Test();

    Task<ApiResponse<IEnumerable<TreeNode>>> GetCompanyTree();
    Task<ApiResponse<IEnumerable<DynamicColumn>>> GetDynamicColumn(int SCtype);
    Task<ApiResponse<bool>> UpdatePayroll(PayrollPeriod period, List<int> employeeIds);

    //Detaisl
    Task<ApiResponse<List<PayrollResult>>> GetListEmployeeSCByPeriod(PayrollPeriod period);
    Task<ApiResponse<PayrollResult>> GetEmployeeDetailSCByPeriod(int payrollId);

    //CRUD SC
    Task<ApiResponse<bool>> UpdateOtherSC(List<PayrollUpsert> lstPayrollUpsert);
    Task<ApiResponse<bool>> UpdatePayrollBonus(List<PayrollUpsert> lstPayrollUpsert);
    Task<ApiResponse<bool>> UpdatePayrollDeduction(List<PayrollUpsert> lstPayrollUpsert);
    Task<ApiResponse<bool>> UpdateFormula(List<PayrollUpsert> lstPayrollUpsert);
    Task<ApiResponse<bool>> UpdatePayrollDetails(PayrollUpsert payrollUpsert);

    //Payroll data
    Task<ApiResponse<List<List<ColumnTableHeader>>>> GetPayrollTableHeader(PayrollPeriod period);
    Task<ApiResponse<IEnumerable<DynamicColumn>>> GetPayrollTableColumn(PayrollPeriod period);

    Task<ApiResponse<IEnumerable<PayrollTableData>>> GetPayrollTableData(PayrollPeriod period, string dfrom, string dto, List<int> employeeIds = null);
    Task<ApiResponse<bool>> SendPayslip(PayrollPeriod period,string dfrom, string dto, List<int> employeeIds);

    //History
    Task<ApiResponse<List<PayrollHistoryModel>>> GetAllPayrollHistory();
    Task<ApiResponse<PayrollHistoryModel>> GetPayrollHistoryDetails(int payrollHistoryId);
    Task<ApiResponse<bool>> SavePayrollHistory(PayrollHistoryModel payrollHistory);
    Task<ApiResponse<bool>> RemovePayrollHistory(int id);
}
public class PayrollsService : IPayrollsService
{
    private string _param_pattern_ = @"\[(.*?)\]";

    private readonly string ConstantPartWageCheckout = "_LUONG_THOI_GIAN";
    private readonly string ConstantPartWageTotalIncome = "_TONG_THU_NHAP";

    private static string fieldNamePrefix = "dp.";
    private readonly string FieldBaseSalary = "PARAM_BASE_SALARY";
    private readonly string FieldBaseHours = "PARAM_BASE_HOURS";
    private readonly string FieldRealHours = "PARAM_REAL_HOURS";
    private readonly string FieldBaseDays = "PARAM_BASE_DAYS";
    private readonly string FieldRealDays = "PARAM_REAL_DAYS";
    private readonly string FieldBaseWageDays = "PARAM_BASE_WAGE_DAYS";
    private readonly string FieldBaseWageHours = "PARAM_BASE_WAGE_HOURS";
    private readonly string FieldBaseFactor = "PARAM_BASE_FACTOR";
    private readonly string FieldHourWageCheckout = "FORMULA_LUONG_THOI_GIAN_HOURS";

    private readonly string FieldOtherBonus = "PARAM_OTHER_BONUS";
    private readonly string FieldTotalAllowance = "PARAM_TOTAL_ALLOWANCE";
    private readonly string FieldTotalBonus = "PARAM_TOTAL_BONUS";
    private readonly string FieldTotalIncome = "FORMULA_TONG_THU_NHAP"; //**FIX

    private readonly string FieldBHXHCheckout = "PARAM_TOTAL_INSURANCE";
    private readonly string FieldTotalPersonalTaxDeduction = "PARAM_TOTAL_PESONAL_TAX_DEDUCTION";
    private readonly string FieldTotalTaxDeduction = "FORMULA_TONG_KHAU_TRU";

    private readonly string FieldNotInTax = "PARAM_MIEN_THUE";
    private readonly string FieldTaxableIncome = "PARAM_TAXALBE_INCOME";
    private readonly string FieldAssessableIncome = "PARAM_ASSESSABLE_INCOME";
    private readonly string FieldTaxRate = "PARAM_TAX_RATE";
    private readonly string FieldTaxCheckout = "PARAM_TOTAL_TAX";


    private readonly string FieldTotalAdvance = "PARAM_TOTAL_ADVANCE";
    private readonly string FieldOtherDeduction = "PARAM_OTHER_DEDUCTION";
    private readonly string FieldTotalDeductionNoTax = "PARAM_TOTAL_DEDUCTION_NOTAX";
    private readonly string FieldAllowanceLunch = "PARAM_ALLOWANCE_LUNCH_NOTAX";
    private readonly string FieldTotalBonusNoTax = "PARAM_TOTAL_BONUS_NOTAX";


    private readonly string Fieldnet = "PARAM_NET";

    #region + Inject Repository
    private readonly IBaseRepository<Payroll> _payrollRepository;
    private readonly IBaseRepository<Department> _departmentRepository;
    private readonly IBaseRepository<Position> _positionRepository;
    private readonly IBaseRepository<Contract> _contractRepository;
    private readonly IBaseRepository<Employee> _employeeRepository;

    private readonly IBaseRepository<Bonus> _bonusRepository;
    private readonly IBaseRepository<Deduction> _deductionRepository;

    private readonly IBaseRepository<Allowance> _allowanceRepository;
    private readonly IBaseRepository<Insurance> _insuranceRepository;
    private readonly IBaseRepository<TaxDeduction> _taxDeductionRepository;

    private readonly IBaseRepository<ContractSalary> _contractSalaryRepository;
    private readonly IBaseRepository<ContractAllowance> _contractAllowanceRepository;
    private readonly IBaseRepository<ContractInsurance> _contractInsuranceRepository;
    private readonly IBaseRepository<TaxDeductionDetails> _taxDeductionDetailsRepository;

    private readonly IBaseRepository<BonusDetails> _bonusDetailsRepository;
    private readonly IBaseRepository<DeductionDetails> _deductionDetailsRepository;

    private readonly IBaseRepository<Advance> _advanceRepository;
    private readonly IBaseRepository<TaxRate> _taxRateRepository;

    private readonly IBaseRepository<Fomula> _fomulaRepository;

    private readonly IEmailService _emailService;
    private readonly IBaseRepository<PayrollHistory> _payrollHistoryRepository;

    private readonly IWorkShiftService _workShiftService;
    public PayrollsService(IBaseRepository<Payroll> payrollRepository,
                           IBaseRepository<Department> departmentRepository,
                           IBaseRepository<Position> positionRepository,
                           IBaseRepository<Contract> contractRepository,
                           IBaseRepository<Employee> employeeRepository,
                           IBaseRepository<Bonus> bonusRepository,
                           IBaseRepository<Deduction> deductionRepository,
                           IBaseRepository<Allowance> allowanceRepository,
                           IBaseRepository<Insurance> insuranceRepository,
                           IBaseRepository<TaxDeduction> taxDeductionRepository,
                           IBaseRepository<ContractAllowance> contractAllowanceRepository,
                           IBaseRepository<ContractSalary> contractSalaryRepository,
                           IBaseRepository<ContractInsurance> contractInsuranceRepository,
                           IBaseRepository<TaxDeductionDetails> taxDeductionDetailsRepository,
                           IBaseRepository<BonusDetails> bonusDetailsRepository,
                           IBaseRepository<DeductionDetails> deductionDetailsRepository,
                           IBaseRepository<Advance> advanceRepository,
                           IBaseRepository<TaxRate> taxRateRepository,
                           IBaseRepository<Fomula> fomulaRepository,
                           IEmailService emailService,
                           IBaseRepository<PayrollHistory> payrollHistoryRepository,
                           IWorkShiftService workShiftService
                           )
    {
        _payrollRepository = payrollRepository;
        _departmentRepository = departmentRepository;
        _positionRepository = positionRepository;
        _contractRepository = contractRepository;
        _employeeRepository = employeeRepository;
        _bonusRepository = bonusRepository;
        _deductionRepository = deductionRepository;
        _allowanceRepository = allowanceRepository;
        _insuranceRepository = insuranceRepository;
        _taxDeductionRepository = taxDeductionRepository;
        _contractSalaryRepository = contractSalaryRepository;
        _contractAllowanceRepository = contractAllowanceRepository;
        _contractInsuranceRepository = contractInsuranceRepository;
        _taxDeductionDetailsRepository = taxDeductionDetailsRepository;
        _bonusDetailsRepository = bonusDetailsRepository;
        _deductionDetailsRepository = deductionDetailsRepository;
        _advanceRepository = advanceRepository;
        _taxRateRepository = taxRateRepository;
        _fomulaRepository = fomulaRepository;
        _emailService = emailService;
        _payrollHistoryRepository = payrollHistoryRepository;
        _workShiftService = workShiftService;

    }

    #endregion

    #region + Add employee to payroll | payslip
    public async Task<ApiResponse<IEnumerable<TreeNode>>> GetCompanyTree()
    {
        try
        {
            var employeeContract = from e in _employeeRepository.GetAllQueryAble()
                                   join c in _contractRepository.GetAllQueryAble() on e.ContractId equals c.Id into contractGroup
                                   from c in contractGroup.DefaultIfEmpty()
                                   select new
                                   {
                                       EmployeeId = e.Id,
                                       ContractId = c != null ? c.Id : (int?)null,
                                       PositionId = c != null ? c.PositionId : (int?)null,
                                       EmployeeName = c != null ? c.Name : null
                                   };

            var departmentPosition = await (from d in _departmentRepository.GetAllQueryAble()
                                            join p in _positionRepository.GetAllQueryAble() on d.Id equals p.DepartmentId into positionGroup
                                            from dp in positionGroup.DefaultIfEmpty()
                                            join ec in employeeContract on dp.Id equals ec.PositionId into ecGroup
                                            from dpec in ecGroup.DefaultIfEmpty()
                                            select new
                                            {
                                                DepartmentId = d.Id,
                                                DepartmentName = d.Name,
                                                PositionId = dp != null ? dp.Id : (int?)null,
                                                PositionName = dp != null ? dp.Name : null,
                                                EmployeeId = dpec != null ? dpec.EmployeeId : (int?)null,
                                                EmployeeName = dpec != null ? dpec.EmployeeName : null
                                            }).ToListAsync();

            // Nhóm theo DepartmentId
            var result = departmentPosition.GroupBy(d => new { d.DepartmentId, d.DepartmentName })
                .Select((g, index) => new TreeNode
                {
                    Key = g.Key.DepartmentId.ToString(),
                    Data = g.Key.DepartmentId.ToString(),
                    Label = g.Key.DepartmentName,
                    Icon = "pi pi-fw pi-inbox",
                    Children = g.GroupBy(p => new { p.PositionId, p.PositionName })
                                .Where(pGroup => pGroup.Key.PositionId != null)
                                .Select((pGroup, pIndex) => new TreeNode
                                {
                                    Key = $"{g.Key.DepartmentId}-{pGroup.Key.PositionId}",
                                    Data = pGroup.Key.PositionId.ToString(),
                                    Label = pGroup.Key.PositionName,
                                    Icon = "pi pi-fw pi-cog", // Icon cho vị trí
                                    Children = pGroup
                                        .Where(emp => emp.EmployeeId != null)
                                        .Select((emp, empIndex) => new TreeNode
                                        {
                                            Key = $"{g.Key.DepartmentId}-{pGroup.Key.PositionId}-{emp.EmployeeId}",
                                            Data = emp.EmployeeId.ToString(),
                                            Label = emp.EmployeeName,
                                            Icon = "pi pi-fw pi-user"
                                        }).ToList()
                                }).ToList()
                }).ToList();

            return new ApiResponse<IEnumerable<TreeNode>>
            {
                Metadata = result,
                IsSuccess = true
            };
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<ApiResponse<bool>> UpdatePayroll(PayrollPeriod period, List<int> employeeIds)
    {
        var employeeIdsOld = _payrollRepository.GetAllQueryAble().Where(x => x.Month == period.Month && x.Year == period.Year).Select(x => x.EmployeeId).ToList();
        var removedIds = employeeIdsOld.Except(employeeIds).ToList();
        var addedIds = employeeIds.Except(employeeIdsOld).ToList();
        using (var transaction = await _payrollRepository.Context.Database.BeginTransactionAsync())
        {

            try
            {
                if (removedIds.Count() > 0)
                {
                    var selectedPayroll = _payrollRepository.GetAllQueryAble().Where(x => x.Month == period.Month && x.Year == period.Year && removedIds.Contains(x.EmployeeId)).ToList();
                    if (selectedPayroll != null && selectedPayroll.Count() > 0)
                    {
                        _payrollRepository.RemoveRange(selectedPayroll);
                    }
                }
                if (addedIds.Count() > 0)
                {
                    var listSelectedEmployee = _employeeRepository.GetAllQueryAble().Where(x => addedIds.Contains(x.Id)).ToList();
                    var listInsertPayroll = new List<Payroll>();
                    foreach (var id in addedIds)
                    {
                        var selectedEmployee = listSelectedEmployee.FirstOrDefault(x => x.Id == id);
                        if (selectedEmployee == null) continue;
                        var payroll = new Payroll()
                        {
                            EmployeeId = id,
                            Month = period.Month,
                            Year = period.Year,
                            ContractId = selectedEmployee.ContractId,
                            OtherDeduction = 0,
                            OtherBonus = 0,
                            FomulaId = 5
                        };
                        listInsertPayroll.Add(payroll);
                    }
                    await _payrollRepository.AddRangeAsync(listInsertPayroll);
                }
                if (removedIds.Count() > 0 || addedIds.Count() > 0)
                {
                    await _payrollRepository.SaveChangeAsync();
                }

                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ApiResponse<bool>()
                {
                    IsSuccess = false,
                    Message = new List<string>() { ex.Message },
                    Metadata = false
                };
            }
        }


        return new ApiResponse<bool>()
        {
            IsSuccess = true,
            Metadata = true
        };
    }

    public async Task<ApiResponse<bool>> SendPayslip(PayrollPeriod period, string dfrom, string dto, List<int> employeeIds)
    {
        try
        {
            var refColumnsApi = await GetPayrollTableColumn(period);
            if (!refColumnsApi.IsSuccess) throw new Exception(refColumnsApi?.Message[0] ?? "Lỗi lấy dữ liệu");
            var refColumns = refColumnsApi?.Metadata?.ToList() ?? new List<DynamicColumn>();

            var lstAllowances = refColumns.Where(x => x.Field.StartsWith("dp.PARAM_ALLOWANCE")).ToList();
            var lstBonus = refColumns.Where(x => x.Field.StartsWith("dp.PARAM_BONUS")).ToList();
            var lstInsurance = refColumns.Where(x => x.Field.StartsWith("dp.PARAM_INSURANCE")).ToList();
            var lstTaxDeduction = refColumns.Where(x => x.Field.StartsWith("dp.PARAM_TAXDEDUCTION")).ToList();
            var lstDeductionNoTax = refColumns.Where(x => x.Field.StartsWith("dp.PARAM_DEDUCTION")).ToList();


            var lstSelectedPayrollRes = await GetPayrollTableData(period, dfrom, dto, employeeIds);
            if (!lstSelectedPayrollRes.IsSuccess) throw new Exception(lstSelectedPayrollRes?.Message[0] ?? "Lỗi lấy dữ liệu");
            var lstPayroll = lstSelectedPayrollRes?.Metadata?.ToList() ?? new List<PayrollTableData>();
            if (lstPayroll == null || lstPayroll.Count == 0)
            {
                throw new Exception(lstSelectedPayrollRes?.Message[0] ?? "Bảng lương trống hoặc không tồn tại nhân viên để gửi phiếu");
            }

            var lstEmail = new List<Email>();

            foreach (var payroll in lstPayroll)
            {
                var htmlAllowanceList = "";
                foreach (var sc in lstAllowances)
                {
                    if (sc.Field.Contains("NOTAX")) continue;
                    htmlAllowanceList += getTemplateSCHtml(sc, payroll.DynamicProperties[sc.Field.Replace("dp.", "")]);
                }

                var htmlBonusList = "";
                foreach (var sc in lstBonus)
                {
                    htmlBonusList += getTemplateSCHtml(sc, payroll.DynamicProperties[sc.Field.Replace("dp.", "")]);
                }

                var htmlInsuranceList = "";
                foreach (var sc in lstInsurance)
                {
                    htmlInsuranceList += getTemplateSCHtml(sc, payroll.DynamicProperties[sc.Field.Replace("dp.", "")]);
                }

                var htmlITaxDeductionList = "";
                foreach (var sc in lstTaxDeduction)
                {
                    htmlITaxDeductionList += getTemplateSCHtml(sc, payroll.DynamicProperties[sc.Field.Replace("dp.", "")]);
                }

                var htmlDeductionNoTaxList = "";
                foreach (var sc in lstDeductionNoTax)
                {
                    htmlDeductionNoTaxList += getTemplateSCHtml(sc, payroll.DynamicProperties[sc.Field.Replace("dp.", "")]);
                }


                var bodyContentEmail = HandleFile.READ_FILE("Email", "Payslip.html")
                 .Replace("{sentDate}", DateTime.Now.ToString("dd/MM/yyyy"))
                 .Replace("{payPeriod}", $"Tháng {(int)period.Month} năm {period.Year}")
                 .Replace("{employeeName}", payroll.EmployeeName)
                 .Replace("{email}", payroll.Email)
                 .Replace("{employeeId}", "NV-00" + payroll.EmployeeId)
                 .Replace("{dateHired}", payroll.DateHired)
                 .Replace("{positionName}", payroll.PositionName)
                 .Replace("{departmentName}", payroll.DepartmentName)
                 .Replace("{taxCode}", "102-231-322-123")
                 .Replace("{" + $"{FieldTotalIncome}" + "}", payroll.DynamicProperties[FieldTotalIncome].ToString("#,##0.00"))
                 .Replace("{" + $"{FieldBaseSalary}" + "}", payroll.DynamicProperties[FieldBaseSalary].ToString("#,##0.00"))
                 .Replace("{" + $"{FieldBaseWageHours}" + "}", payroll.DynamicProperties[FieldBaseWageHours].ToString("#,##0.00") + "/h")
                 .Replace("{" + $"{FieldBaseHours}" + "}", payroll.DynamicProperties[FieldBaseHours].ToString())
                 .Replace("{" + $"{FieldRealHours}" + "}", payroll.DynamicProperties[FieldRealHours].ToString())
                 .Replace("{" + $"{FieldHourWageCheckout}" + "}", payroll.DynamicProperties[FieldHourWageCheckout].ToString("#,##0.00"))
                 .Replace("{" + $"{FieldOtherBonus}" + "}", payroll.DynamicProperties[FieldOtherBonus].ToString("#,##0.00"))
                 .Replace("{" + $"{FieldTotalIncome}" + "}", payroll.DynamicProperties[FieldTotalIncome].ToString("#,##0.00"))
                 .Replace("{" + $"{FieldTotalTaxDeduction}" + "}", payroll.DynamicProperties[FieldTotalTaxDeduction].ToString("#,##0.00"))
                 .Replace("{" + $"{FieldTaxCheckout}" + "}", payroll.DynamicProperties[FieldTaxCheckout].ToString("#,##0.00"))
                 .Replace("{" + $"{FieldTotalAdvance}" + "}", payroll.DynamicProperties[FieldTotalAdvance].ToString("#,##0.00"))
                 .Replace("{" + $"{FieldOtherDeduction}" + "}", payroll.DynamicProperties[FieldRealHours].ToString("#,##0.00"))
                 .Replace("{" + $"{Fieldnet}" + "}", payroll.DynamicProperties[Fieldnet].ToString("#,##0.00"))
                 .Replace("{allowanceList}", htmlAllowanceList)
                 .Replace("{bonusList}", htmlBonusList)
                 .Replace("{insuranceList}", htmlInsuranceList)
                 .Replace("{taxDeductionList}", htmlITaxDeductionList)
                 .Replace("{deductionNoTaxList}", htmlDeductionNoTaxList)
                 .Replace("{bonusNoTaxList}", "");
                var bodyEmail = _emailService.TemplateContent
                                .Replace("<main>", "")
                                .Replace("/<main>", "")
                                .Replace("{content}", bodyContentEmail);

                var email = new Email()
                {
                    To = payroll.Email,
                    Body = bodyEmail,
                    Subject = $"[HRM] - PHIẾU LƯƠNG THÁNG {period.Month} NĂM {period.Year}"
                };
                lstEmail.Add(email);

            }

            foreach (var email in lstEmail)
            {
                await _emailService.SendEmailToRecipient(email);
            }
        }
        catch (Exception)
        {

            throw;
        }

        return new ApiResponse<bool>()
        {
            IsSuccess = true,
            Metadata = true
        };
    }

    private string getTemplateSCHtml(DynamicColumn column, double value)
    {
        var res = @$"<div class=""entry"" style=""display: flex; justify-content: space-between; margin-bottom: 6px;"">
                            <div class=""label"" style=""font-weight: 700; width: 120px;""></div>
                            <div class=""detail"" style=""font-weight: 600; width: 130px;"">{column.Header}</div>
                            <div class=""rate"" style=""font-weight: 400; width: 80px; font-style: italic; letter-spacing: 1px;""></div>
                            <div class="""" style=""text-align: right; width: 170px;"">{value.ToString("#,##0.00")}</div>
                        </div>";


        return res;
    }

    #endregion

    #region + CRUD SC Components
    public async Task<ApiResponse<List<PayrollResult>>> GetListEmployeeSCByPeriod(PayrollPeriod period)
    {
        try
        {
            var payrollListResult = (from p in _payrollRepository.GetAllQueryAble().Where(x => x.Month == period.Month && x.Year == period.Year)
                                     join e in _employeeRepository.GetAllQueryAble() on p.EmployeeId equals e.Id
                                     join d in _departmentRepository.GetAllQueryAble() on p.Contract.Position.DepartmentId equals d.Id
                                     select new PayrollResult()
                                     {
                                         Id = p.Id,
                                         Month = p.Month,
                                         Year = p.Year,
                                         EmployeeId = p.EmployeeId,
                                         ContractId = p.ContractId,
                                         EmployeeName = (p.Contract != null) ? (p.Contract.Name ?? "") : "",
                                         PositionName = (p.Contract != null && p.Contract.Position != null) ? p.Contract.Position.Name : "",
                                         DepartmentName = d.Name,
                                         OtherDeduction = p.OtherDeduction,
                                         OtherBonus = p.OtherBonus,
                                         FomulaId = p.FomulaId,
                                         ListBonusIds = (p.bonusDetails != null) ? p.bonusDetails.Select(x => x.BonusId).ToList() : new List<int>(),
                                         ListDeductionIds = (p.DeductionDetails != null) ? p.DeductionDetails.Select(x => x.DeductionId).ToList() : new List<int>(),

                                     }).ToList();

            return new ApiResponse<List<PayrollResult>>()
            {
                IsSuccess = true,
                Metadata = payrollListResult ?? new List<PayrollResult>()
            };
        }
        catch (Exception e)
        {

            return new ApiResponse<List<PayrollResult>>()
            {
                IsSuccess = false,
                Message = new List<string>() { e.Message }
            };
        }


    }

    public async Task<ApiResponse<PayrollResult>> GetEmployeeDetailSCByPeriod(int payrollId)
    {
        try
        {
            var selectedPayroll = _payrollRepository.GetAllQueryAble().FirstOrDefault(x => x.Id == payrollId);
            if (selectedPayroll == null)
            {
                throw new Exception("Không tồn tại bảng lương");
            }
            var bonusListIds = _bonusDetailsRepository.GetAllQueryAble().Where(x => x.PayrollId == selectedPayroll.Id).Select(x => x.BonusId).ToList();
            var deductionListIds = _deductionDetailsRepository.GetAllQueryAble().Where(x => x.PayrollId == selectedPayroll.Id).Select(x => x.DeductionId).ToList();

            var selectedContract = _contractRepository.GetAllQueryAble().FirstOrDefault(x => x.Id == selectedPayroll.ContractId);

            var contractSalaryResult = new ContractSalaryResult();
            var allowanceResult = new List<AllowanceResult>();
            var insuranceResult = new List<InsuranceResult>();



            var selectedEmployee = _employeeRepository.GetAllQueryAble().FirstOrDefault(x => x.Id == selectedPayroll.EmployeeId);
            var taxDeduction = new List<TaxDeductionResult>();

            var departmentName = "";
            if (selectedContract != null)
            {
                var department = _departmentRepository.GetAllQueryAble();
                var position = _positionRepository.GetAllQueryAble().FirstOrDefault(x => x.Id == selectedContract.PositionId);
                departmentName = position != null ? department.FirstOrDefault(x => x.Id == position.DepartmentId)?.Name ?? "" : "";

                var contractSalary = _contractSalaryRepository.GetAllQueryAble().FirstOrDefault(x => x.Id == selectedContract.ContractSalaryId);
                if (contractSalary != null)
                {
                    contractSalaryResult = new ContractSalaryResult()
                    {
                        BaseSalary = contractSalary.BaseSalary,
                        BaseInsurance = contractSalary.BaseInsurance,
                        RequiredDays = contractSalary.RequiredDays,
                        RequiredHours = contractSalary.RequiredHours,
                        WageDaily = contractSalary.WageDaily,
                        WageHourly = contractSalary.WageHourly,
                        Factor = contractSalary.Factor
                    };
                }

                var listAllowanceIds = _contractAllowanceRepository.GetAllQueryAble().Where(x => x.ContractId == selectedContract.Id).Select(x => x.AllowanceId).ToList();
                allowanceResult = _allowanceRepository.GetAllQueryAble().Where(x => listAllowanceIds.Contains(x.Id))
                                   .Select(a => new AllowanceResult()
                                   {
                                       Id = a.Id,
                                       Name = a.Name,
                                       Amount = a.Amount
                                   }).ToList();

                var listInsuranceIds = _contractInsuranceRepository.GetAllQueryAble().Where(x => x.ContractId == selectedContract.Id).Select(x => x.InsuranceId).ToList();
                insuranceResult = _insuranceRepository.GetAllQueryAble().Where(x => listInsuranceIds.Contains(x.Id)).Select(i => new InsuranceResult()
                {
                    Id = i.Id,
                    Name = i.Name,
                    PercentEmployee = i.PercentEmployee,
                    PercentCompany = i.PercentCompany
                }).ToList();



            }
            if (selectedEmployee != null)
            {
                var listTaxDeductionIds = _taxDeductionDetailsRepository.GetAllQueryAble().Where(x => x.EmployeeId == selectedEmployee.Id).Select(x => x.TaxDeductionId).ToList();
                taxDeduction = _taxDeductionRepository.GetAllQueryAble()
                                        .Where(x => listTaxDeductionIds.Contains(x.Id))
                                        .Select(a => new TaxDeductionResult()
                                        {
                                            Id = a.Id,
                                            Name = a.Name,
                                            Amount = a.Amount
                                        }).ToList();

            }
            var payrollResult = new PayrollResult()
            {
                Id = selectedPayroll.Id,
                Month = selectedPayroll.Month,
                Year = selectedPayroll.Year,
                EmployeeId = selectedPayroll.EmployeeId,
                ContractId = selectedPayroll.ContractId,
                EmployeeName = (selectedPayroll.Contract != null) ? (selectedPayroll.Contract.Name ?? "") : "",
                PositionName = (selectedPayroll.Contract != null && selectedPayroll.Contract.Position != null) ? selectedPayroll.Contract.Position.Name : "",
                DepartmentName = departmentName,
                OtherDeduction = selectedPayroll.OtherDeduction,
                OtherBonus = selectedPayroll.OtherBonus,
                FomulaId = selectedPayroll.FomulaId,
                ListBonusIds = bonusListIds ?? new List<int>(),
                ListDeductionIds = deductionListIds ?? new List<int>(),
                ContractSalary = contractSalaryResult,
                ListAllowance = allowanceResult,
                ListInsurance = insuranceResult,
                ListTaxDeduction = taxDeduction ?? new List<TaxDeductionResult>()
            };

            return new ApiResponse<PayrollResult>()
            {
                IsSuccess = true,
                Metadata = payrollResult ?? new PayrollResult()
            };
        }
        catch (Exception e)
        {

            return new ApiResponse<PayrollResult>()
            {
                IsSuccess = false,
                Message = new List<string>() { e.Message }
            };
        }

    }

    public async Task<ApiResponse<bool>> UpdateOtherSC(List<PayrollUpsert> lstPayrollUpsert)
    {
        try
        {
            var lstPayrollIds = lstPayrollUpsert.Select(x => x.Id).ToList();
            var selectedListPayrollOld = await _payrollRepository.GetAllQueryAble().Where(x => lstPayrollIds.Contains(x.Id)).ToListAsync();
            for (int i = 0; i < selectedListPayrollOld.Count; i++)
            {
                var seletedUpdatePayroll = lstPayrollUpsert.FirstOrDefault(x => x.Id == selectedListPayrollOld[i].Id);
                if (seletedUpdatePayroll != null)
                {
                    selectedListPayrollOld[i].OtherBonus = seletedUpdatePayroll.OtherBonus;
                    selectedListPayrollOld[i].OtherDeduction = seletedUpdatePayroll.OtherDeduction;
                }

            }
            _payrollRepository.UpdateMany(selectedListPayrollOld);
            await _payrollRepository.SaveChangeAsync();
            return new ApiResponse<bool>()
            {
                IsSuccess = true,
                Metadata = true
            };
        }
        catch (Exception e)
        {
            return new ApiResponse<bool>()
            {
                IsSuccess = false,
                Metadata = false,
                Message = new List<string>() { e.Message }
            };
        }

    }

    public async Task<ApiResponse<bool>> UpdatePayrollBonus(List<PayrollUpsert> lstPayrollUpsert)
    {
        using (var transaction = await _bonusDetailsRepository.Context.Database.BeginTransactionAsync())
        {
            try
            {
                var lstPayrollIds = lstPayrollUpsert.Select(x => x.Id).ToList();
                if (lstPayrollIds == null) throw new Exception("lst payrol undefined");
                //Bonus
                var listOldDetails = await _bonusDetailsRepository.GetAllQueryAble().Where(x => lstPayrollIds.Contains(x.PayrollId)).ToListAsync();
                var listAllAdd = new List<BonusDetails>();
                var listAllRemove = new List<BonusDetails>();
                foreach (var payrolUpdate in lstPayrollUpsert)
                {
                    var payrollOldList = listOldDetails.Where(x => x.PayrollId == payrolUpdate.Id);

                    var payrollNewListIds = payrolUpdate.ListBonusIds;
                    var payrollOldListIds = payrollOldList.Select(x => x.BonusId).ToList();

                    var removedIds = payrollOldListIds.Except(payrollNewListIds).ToList();
                    var addedIds = payrollNewListIds.Except(payrollOldListIds).ToList();
                    if (removedIds.Count > 0)
                    {
                        var selectedDetails = payrollOldList.Where(x => removedIds.Contains(x.BonusId)).ToList();
                        if (selectedDetails != null && selectedDetails.Count() > 0)
                        {
                            listAllRemove.AddRange(selectedDetails);
                        }
                    }
                    if (addedIds.Count > 0)
                    {
                        foreach (var id in addedIds)
                        {
                            var details = new BonusDetails()
                            {
                                BonusId = id,
                                PayrollId = payrolUpdate.Id
                            };
                            listAllAdd.Add(details);
                        }
                    }

                }
                if (listAllRemove.Count > 0)
                {
                    _bonusDetailsRepository.RemoveRange(listAllRemove);
                }
                if (listAllAdd.Count > 0)
                {

                    await _bonusDetailsRepository.AddRangeAsync(listAllAdd);
                }

                if (listAllRemove.Count() > 0 || listAllAdd.Count() > 0)
                {
                    await _bonusDetailsRepository.SaveChangeAsync();
                }
                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return new ApiResponse<bool>()
                {
                    IsSuccess = false,
                    Metadata = false,
                    Message = new List<string>() { e.Message }
                };
            }

        }


        return new ApiResponse<bool>()
        {
            IsSuccess = true,
            Metadata = true
        };


    }

    public async Task<ApiResponse<bool>> UpdatePayrollDeduction(List<PayrollUpsert> lstPayrollUpsert)
    {

        using (var transaction = await _deductionDetailsRepository.Context.Database.BeginTransactionAsync())
        {
            try
            {
                var lstPayrollIds = lstPayrollUpsert.Select(x => x.Id).ToList();
                if (lstPayrollIds == null) throw new Exception("lst payrol undefined");
                var listOldDetails = await _deductionDetailsRepository.GetAllQueryAble().Where(x => lstPayrollIds.Contains(x.PayrollId)).ToListAsync();
                var listAllAdd = new List<DeductionDetails>();
                var listAllRemove = new List<DeductionDetails>();
                foreach (var payrolUpdate in lstPayrollUpsert)
                {
                    var payrollOldList = listOldDetails.Where(x => x.PayrollId == payrolUpdate.Id);

                    var payrollNewListIds = payrolUpdate.ListDeductionIds;
                    var payrollOldListIds = payrollOldList.Select(x => x.DeductionId).ToList();

                    var removedIds = payrollOldListIds.Except(payrollNewListIds).ToList();
                    var addedIds = payrollNewListIds.Except(payrollOldListIds).ToList();
                    if (removedIds.Count > 0)
                    {
                        var selectedDetails = payrollOldList.Where(x => removedIds.Contains(x.DeductionId)).ToList();
                        if (selectedDetails != null && selectedDetails.Count() > 0)
                        {
                            listAllRemove.AddRange(selectedDetails);

                        }
                    }
                    if (addedIds.Count > 0)
                    {
                        foreach (var id in addedIds)
                        {
                            var details = new DeductionDetails()
                            {
                                DeductionId = id,
                                PayrollId = payrolUpdate.Id
                            };
                            listAllAdd.Add(details);
                        }
                    }

                }
                if (listAllRemove.Count > 0)
                {
                    _deductionDetailsRepository.RemoveRange(listAllRemove);
                }
                if (listAllAdd.Count > 0)
                {

                    await _deductionDetailsRepository.AddRangeAsync(listAllAdd);
                }

                if (listAllRemove.Count() > 0 || listAllAdd.Count() > 0)
                {
                    await _deductionDetailsRepository.SaveChangeAsync();
                }
                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return new ApiResponse<bool>()
                {
                    IsSuccess = false,
                    Metadata = false,
                    Message = new List<string>() { e.Message }
                };
            }

        }

        return new ApiResponse<bool>()
        {
            IsSuccess = true,
            Metadata = true
        };


    }

    public async Task<ApiResponse<bool>> UpdateFormula(List<PayrollUpsert> lstPayrollUpsert)
    {
        try
        {
            var lstPayrollIds = lstPayrollUpsert.Select(x => x.Id).ToList();
            var selectedListPayrollOld = await _payrollRepository.GetAllQueryAble().Where(x => lstPayrollIds.Contains(x.Id)).ToListAsync();
            for (int i = 0; i < selectedListPayrollOld.Count; i++)
            {
                var seletedUpdatePayroll = lstPayrollUpsert.FirstOrDefault(x => x.Id == selectedListPayrollOld[i].Id);
                if (seletedUpdatePayroll != null)
                {
                    selectedListPayrollOld[i].FomulaId = seletedUpdatePayroll.FomulaId;
                }

            }
            _payrollRepository.UpdateMany(selectedListPayrollOld);
            await _payrollRepository.SaveChangeAsync();
            return new ApiResponse<bool>()
            {
                IsSuccess = true,
                Metadata = true
            };
        }
        catch (Exception e)
        {
            return new ApiResponse<bool>()
            {
                IsSuccess = false,
                Metadata = false,
                Message = new List<string>() { e.Message }
            };
        }

    }

    public async Task<ApiResponse<bool>> UpdatePayrollDetails(PayrollUpsert payrollUpsert)
    {
        using (var transaction = await _bonusDetailsRepository.Context.Database.BeginTransactionAsync())
        {
            try
            {
                //Bonus
                var listOldBonusDetails = await _bonusDetailsRepository.GetAllQueryAble().Where(x => payrollUpsert.Id == x.PayrollId).ToListAsync();
                var listAllBonusAdd = new List<BonusDetails>();
                var listAllBonusRemove = new List<BonusDetails>();

                var payrollOldBonusList = listOldBonusDetails.Where(x => x.PayrollId == payrollUpsert.Id);

                var payrollNewListBonusIds = payrollUpsert.ListBonusIds;
                var payrollOldListBonusIds = payrollOldBonusList.Select(x => x.BonusId).ToList();

                var removedBonusIds = payrollOldListBonusIds.Except(payrollNewListBonusIds).ToList();
                var addedBonusIds = payrollNewListBonusIds.Except(payrollOldListBonusIds).ToList();
                if (removedBonusIds.Count > 0)
                {
                    var selectedDetails = payrollOldBonusList.Where(x => removedBonusIds.Contains(x.BonusId)).ToList();
                    if (selectedDetails != null && selectedDetails.Count() > 0)
                    {
                        listAllBonusRemove.AddRange(selectedDetails);
                    }
                }
                if (addedBonusIds.Count > 0)
                {
                    foreach (var id in addedBonusIds)
                    {
                        var details = new BonusDetails()
                        {
                            BonusId = id,
                            PayrollId = payrollUpsert.Id
                        };
                        listAllBonusAdd.Add(details);
                    }
                }

                if (listAllBonusRemove.Count > 0)
                {
                    _bonusDetailsRepository.RemoveRange(listAllBonusRemove);
                }
                if (listAllBonusAdd.Count > 0)
                {

                    await _bonusDetailsRepository.AddRangeAsync(listAllBonusAdd);
                }

                //Deduction
                var listOldDeductionDetails = await _deductionDetailsRepository.GetAllQueryAble().Where(x => payrollUpsert.Id == x.PayrollId).ToListAsync();
                var listAllDeductionAdd = new List<DeductionDetails>();
                var listAllDeductionRemove = new List<DeductionDetails>();

                var payrollOldList = listOldDeductionDetails.Where(x => x.PayrollId == payrollUpsert.Id);

                var payrollNewListDeductionIds = payrollUpsert.ListDeductionIds;
                var payrollOldListDeductionIds = payrollOldList.Select(x => x.DeductionId).ToList();

                var removedDeductionIds = payrollOldListDeductionIds.Except(payrollNewListDeductionIds).ToList();
                var addedDeductionIds = payrollNewListDeductionIds.Except(payrollOldListDeductionIds).ToList();
                if (removedDeductionIds.Count > 0)
                {
                    var selectedDetails = payrollOldList.Where(x => removedDeductionIds.Contains(x.DeductionId)).ToList();
                    if (selectedDetails != null && selectedDetails.Count() > 0)
                    {
                        listAllDeductionRemove.AddRange(selectedDetails);
                    }
                }
                if (addedDeductionIds.Count > 0)
                {
                    foreach (var id in addedDeductionIds)
                    {
                        var details = new DeductionDetails()
                        {
                            DeductionId = id,
                            PayrollId = payrollUpsert.Id
                        };
                        listAllDeductionAdd.Add(details);
                    }
                }

                if (listAllDeductionRemove.Count > 0)
                {
                    _deductionDetailsRepository.RemoveRange(listAllDeductionRemove);
                }
                if (listAllDeductionAdd.Count > 0)
                {

                    await _deductionDetailsRepository.AddRangeAsync(listAllDeductionAdd);
                }

                if (listAllBonusRemove.Count() > 0 || listAllBonusAdd.Count() > 0)
                {
                    await _bonusDetailsRepository.SaveChangeAsync();
                }

                if (listAllDeductionRemove.Count() > 0 || listAllDeductionAdd.Count() > 0)
                {
                    await _deductionDetailsRepository.SaveChangeAsync();
                }

                await transaction.CommitAsync();
            }
            catch (Exception e)
            {
                await transaction.RollbackAsync();
                return new ApiResponse<bool>()
                {
                    IsSuccess = false,
                    Metadata = false,
                    Message = new List<string>() { e.Message }
                };
            }

        }


        return new ApiResponse<bool>()
        {
            IsSuccess = true,
            Metadata = true
        };


    }
    #endregion

    #region + Update payroll of employee: bonus, deductions, fomula, other bonus, deductions
    public async Task<ApiResponse<IEnumerable<DynamicColumn>>> GetDynamicColumn(int SCtype)
    {
        var res = new List<DynamicColumn>();
        try
        {
            //Bonus
            if (SCtype == 0)
            {
                res = await _bonusRepository.GetAllQueryAble().Select(x => new DynamicColumn()
                {
                    Field = x.Id.ToString(),
                    Header = $"{x.Name} ({x.Amount})",
                }).ToListAsync();
            }
            else if (SCtype == 1) // Deduction
            {
                res = await _deductionRepository.GetAllQueryAble().Select(x => new DynamicColumn()
                {
                    Field = x.Id.ToString(),
                    Header = $"{x.Name} ({x.Amount})",
                }).ToListAsync();
            }
            res.Insert(0, new DynamicColumn()
            {
                Field = "employeeName",
                Header = "Tên nhân viên"
            });
        }
        catch (Exception e)
        {
            return new ApiResponse<IEnumerable<DynamicColumn>>()
            {
                IsSuccess = false,
                Metadata = new List<DynamicColumn>(),
                Message = new List<string>() { e.Message }

            };
        }

        return new ApiResponse<IEnumerable<DynamicColumn>>()
        {
            IsSuccess = true,
            Metadata = res
        };
    }

    #endregion

    // Update bonus, deductions, fomula for many employees

    #region + Payroll Calculation
    public async Task<ApiResponse<List<List<ColumnTableHeader>>>> GetPayrollTableHeader(PayrollPeriod period)
    {
        var payrollByPeriod = _payrollRepository.GetAllQueryAble().Where(x => x.Year == period.Year && x.Month == period.Month);
        var lstContractIds = payrollByPeriod.Select(x => x.ContractId).ToList(); ;
        var lstEmployeeIds = payrollByPeriod.Select(x => x.EmployeeId).ToList(); ;
        var lstPayrollIds = payrollByPeriod.Select(x => x.Id).ToList(); ;


        var taskAllowance = getListAllowanceInPeriod(lstContractIds);
        var taskInsurance = getListInsuranceInPeriod(lstContractIds);
        var taskTaxDeduction = getListTaxDeductionInPeriod(lstEmployeeIds);
        var taskBonus = getListBonusInPeriod(lstPayrollIds);
        var taskDeduction = getListDeductionInPeriod(lstPayrollIds);

        Task.WaitAll(taskAllowance, taskInsurance, taskTaxDeduction, taskBonus, taskDeduction);
        var allowanceColumn = taskAllowance.Result;
        var insuranceColumn = taskInsurance.Result;
        var taxDeductionColumn = taskTaxDeduction.Result;
        var bonusColumn = taskBonus.Result;
        var deductionColumn = taskDeduction.Result;

        var res = new List<List<ColumnTableHeader>>();

        res.Add(new List<ColumnTableHeader>()
        {
            new ColumnTableHeader()
            {
                Id = (int)ColId.ColIdEmployeeName,
                Header = "Họ và tên",
                Field = "employeeName",
                RowSpan = 3
            },
            new ColumnTableHeader()
            {
                Id = (int)ColId.ColIdDepartmentName,
                Header = "Phòng ban",
                Field = "departmentName",
                RowSpan = 3
            },
            new ColumnTableHeader()
            {
                Id = (int)ColId.ColIdBaseSalary,
                Header = "Lương cơ bản",
                Field = FieldBaseSalary,
                RowSpan = 3
            },
            new ColumnTableHeader()
            {
                Id = (int)ColId.ColIdSCIncome,
                Header = "Tổng thu nhập",
                ColSpan = 2+allowanceColumn.Count+bonusColumn.Count +1+1,
            },
            new ColumnTableHeader()
            {
                Id = (int)ColId.ColIdSCDeduction,
                Header = "Khấu trừ",
                ColSpan = insuranceColumn.Count+1 +taxDeductionColumn.Count+1,
            },
            new ColumnTableHeader()
            {
                Id = (int)ColId.ColIdSCTax,
                Header = "Thuế TNCN",
                ColSpan = (1)+1+1+1+1
            },
            new ColumnTableHeader()
            {
                Id = (int)ColId.ColIdSCNotax,
                Header = "Các khoản không tính thuế ",
                ColSpan = 1+deductionColumn.Count + 1 + 1 + (1) + 1
            },
            new ColumnTableHeader()
            {
                Id = (int)ColId.ColIdSCNet,
                Header = "Thực lĩnh",
                RowSpan = 3,
                Field = Fieldnet
            },
        });

        var secColumn = new List<ColumnTableHeader>();
        secColumn.Add(new ColumnTableHeader()
        {
            Id = (int)ColId.ColIdHourWage,
            ListParentIds = new List<int>() { (int)ColId.ColIdSCIncome },
            Header = "Lương thời gian",
            ColSpan = 2
        });

        if (allowanceColumn.Count != 0)
        {
            secColumn.Add(new ColumnTableHeader()
            {
                Id = (int)ColId.ColIdAllAllowance,
                ListParentIds = new List<int>() { (int)ColId.ColIdSCIncome },
                Header = "Phụ cấp",
                ColSpan = allowanceColumn.Count
            });
        }

        secColumn.AddRange(new List<ColumnTableHeader>()
        {
            new ColumnTableHeader()
            {
                Id = (int)ColId.ColIdAllBonus,
                ListParentIds = new List<int>() {(int)ColId.ColIdSCIncome },
                Header = "Các khoản thưởng",
                ColSpan = bonusColumn.Count + 1
            },
            new ColumnTableHeader()
            {
                Id = (int)ColId.ColIdTotalIncome,
                ListParentIds = new List<int>() {(int)ColId.ColIdSCIncome },
                Header = "Thành tiền",
                Field= FieldTotalIncome,
                RowSpan = 2
            },
        });


        secColumn.Add(new ColumnTableHeader()
        {
            Id = (int)ColId.ColIdAllInsurance,
            ListParentIds = new List<int>() { (int)ColId.ColIdSCDeduction },
            Header = "Bảo hiểm",
            ColSpan = insuranceColumn.Count + 1,
            RowSpan = insuranceColumn.Count == 0 ? 2 : 1
        });

        if (taxDeductionColumn.Count > 0)
        {
            secColumn.Add(new ColumnTableHeader()
            {
                Id = (int)ColId.ColIdAllTaxDeduction,
                Header = "Giảm trừ gia cảnh",
                ListParentIds = new List<int>() { (int)ColId.ColIdSCDeduction },
                ColSpan = taxDeductionColumn.Count,
            });
        }

        secColumn.AddRange(new List<ColumnTableHeader>()
        {

            new ColumnTableHeader()
            {
                Id = (int)ColId.ColIdTotalDeduction,
                Header = "Thành tiền",
                ListParentIds = new List<int>() { (int)ColId.ColIdSCDeduction },
                Field = FieldTotalTaxDeduction,
                RowSpan = 2
            },
            new ColumnTableHeader()
            {
                Id = (int)ColId.ColIdNotInTax,
                Header = "Các khoản miễn thuế",
                ListParentIds = new List<int>() { (int)ColId.ColIdSCTax },
                Field = FieldNotInTax ,
                RowSpan = 2
            },
            new ColumnTableHeader()
            {
                Id = (int)ColId.ColIdTaxIncome,
                Header = "Thu nhập chịu thuế",
                ListParentIds = new List<int>() { (int)ColId.ColIdSCTax },
                Field = FieldTaxableIncome,
                RowSpan = 2
            },
            new ColumnTableHeader()
            {
                Id = (int)ColId.ColIdAccessableIncome,
                Header = "Thu nhập tính thuế",
                ListParentIds = new List<int>() { (int)ColId.ColIdSCTax },
                Field = FieldAssessableIncome,
                RowSpan = 2
            },
            new ColumnTableHeader()
            {
                Id = (int)ColId.ColIdTaxrate,
                Header = "% Thuế xuất",
                ListParentIds = new List<int>() { (int)ColId.ColIdSCTax },
                Field = FieldTaxRate,
                RowSpan = 2
            },
            new ColumnTableHeader()
            {
                Id = (int)ColId.ColIdTaxCheckout,
                Header = "Thành tiền",
                ListParentIds = new List<int>() { (int)ColId.ColIdSCTax },
                RowSpan = 2,
                Field = FieldTaxCheckout
            }
        });

        secColumn.AddRange(new List<ColumnTableHeader>()
        {
            new ColumnTableHeader()
            {
                Id = (int)ColId.ColIdAdvance,
                ListParentIds = new List<int>() { (int)ColId.ColIdSCNotax },
                Header = "Tạm ứng",
                Field = FieldTotalAdvance,
                RowSpan = 2
            },
            new ColumnTableHeader()
            {
                Id = (int)ColId.ColIdAllDeductionNoTax,
                ListParentIds = new List<int>() { (int)ColId.ColIdSCNotax },
                Header = "Khoản trừ",
                ColSpan = deductionColumn.Count + 1 + 1
            },
             new ColumnTableHeader()
            {
                Id = (int)ColId.ColIdAllBonusNoTax,
                ListParentIds = new List<int>() { (int)ColId.ColIdSCNotax },
                Header = "Khoản cộng/phụ cấp",
                ColSpan = (1)+1
            }

        });


        res.Add(secColumn);


        //3rd row
        var bottomColumn = new List<ColumnTableHeader>()
        {
            new ColumnTableHeader()
            {
                ListParentIds = new List<int>() { (int)ColId.ColIdSCIncome, (int)ColId.ColIdHourWage },
                Header = "Số công (giờ)",
                Field = FieldRealHours,
            },
            new ColumnTableHeader()
            {
                ListParentIds = new List<int>() { (int)ColId.ColIdSCIncome, (int)ColId.ColIdHourWage },
                Header = "Thành tiền",
                Field = FieldHourWageCheckout,
            }
        };

        //Allowance
        if (allowanceColumn.Count > 0)
        {
            bottomColumn.AddRange(allowanceColumn.Select(x => new ColumnTableHeader() { Header = x.Header, Field = x.Field, ListParentIds = new List<int>() { (int)ColId.ColIdSCIncome, (int)ColId.ColIdAllAllowance }, }));
        }

        //Bonus
        if (bottomColumn.Count > 0)
        {
            bottomColumn.AddRange(bonusColumn.Select(x => new ColumnTableHeader() { Header = x.Header, Field = x.Field, ListParentIds = new List<int>() { (int)ColId.ColIdSCIncome, (int)ColId.ColIdAllBonus }, }));
        }
        bottomColumn.Add(new ColumnTableHeader() { Header = "Thưởng khác", Field = FieldOtherBonus, ListParentIds = new List<int>() { (int)ColId.ColIdSCIncome, (int)ColId.ColIdAllBonus }, });

        //BHXH
        if (insuranceColumn.Count > 0)
        {
            bottomColumn.AddRange(insuranceColumn.Select(x => new ColumnTableHeader() { Header = x.Header, Field = x.Field, ListParentIds = new List<int>() { (int)ColId.ColIdSCDeduction, (int)ColId.ColIdAllInsurance }, }));
            bottomColumn.Add(new ColumnTableHeader()
            {
                Header = "Thành tiền",
                Field = FieldBHXHCheckout,
                ListParentIds = new List<int>() { (int)ColId.ColIdSCDeduction, (int)ColId.ColIdAllInsurance },
            });
        }

        //Tax deduction
        if (taxDeductionColumn.Count > 0)
        {
            bottomColumn.AddRange(taxDeductionColumn.Select(x => new ColumnTableHeader() { Header = x.Header, Field = x.Field, ListParentIds = new List<int>() { (int)ColId.ColIdSCDeduction, (int)ColId.ColIdAllTaxDeduction } }));
        }

        // Deduction no tax
        if (deductionColumn.Count > 0)
        {
            bottomColumn.AddRange(deductionColumn.Select(x => new ColumnTableHeader() { Header = x.Header, Field = x.Field, ListParentIds = new List<int>() { (int)ColId.ColIdSCNotax, (int)ColId.ColIdAllDeductionNoTax } }));
        }
        bottomColumn.Add(new ColumnTableHeader() { Header = "Trừ khác", Field = FieldOtherDeduction, ListParentIds = new List<int>() { (int)ColId.ColIdSCNotax, (int)ColId.ColIdAllDeductionNoTax } });
        bottomColumn.Add(new ColumnTableHeader() { Header = "Thành tiền", Field = FieldTotalDeductionNoTax, ListParentIds = new List<int>() { (int)ColId.ColIdSCNotax, (int)ColId.ColIdAllDeductionNoTax } });

        // Bonus No tax
        bottomColumn.Add(new ColumnTableHeader() { Header = "Ăn ca", Field = FieldAllowanceLunch, ListParentIds = new List<int>() { (int)ColId.ColIdSCNotax, (int)ColId.ColIdAllBonusNoTax } });
        bottomColumn.Add(new ColumnTableHeader() { Header = "Thành tiền", Field = FieldTotalBonusNoTax, ListParentIds = new List<int>() { (int)ColId.ColIdSCNotax, (int)ColId.ColIdAllBonusNoTax } });
        res.Add(bottomColumn);

        for (int i = 0; i < res.Count; i++)
        {

            for (int j = 0; j < res[i].Count; j++)
            {
                if (i == res.Count - 1)
                {
                    res[i][j].Id = 1000 + j;
                }
                if (res[i][j].Field.Contains("PARAM_") || res[i][j].Field.Contains("PARAMS_") || res[i][j].Field.Contains("FORMULA_"))
                {
                    res[i][j].Field = fieldNamePrefix + res[i][j].Field;
                }
            }

        }



        return new ApiResponse<List<List<ColumnTableHeader>>>()
        {
            IsSuccess = true,
            Metadata = res
        };
    }
    public async Task<ApiResponse<IEnumerable<DynamicColumn>>> GetPayrollTableColumn(PayrollPeriod period)
    {
        var payrollByPeriod = _payrollRepository.GetAllQueryAble().Where(x => x.Year == period.Year && x.Month == period.Month);
        var lstContractIds = payrollByPeriod.Select(x => x.ContractId).ToList(); ;
        var lstEmployeeIds = payrollByPeriod.Select(x => x.EmployeeId).ToList(); ;
        var lstPayrollIds = payrollByPeriod.Select(x => x.Id).ToList(); ;

        var taskAllowance = getListAllowanceInPeriod(lstContractIds);
        var taskInsurance = getListInsuranceInPeriod(lstContractIds);
        var taskTaxDeduction = getListTaxDeductionInPeriod(lstEmployeeIds);
        var taskBonus = getListBonusInPeriod(lstPayrollIds);
        var taskDeduction = getListDeductionInPeriod(lstPayrollIds);

        Task.WaitAll(taskAllowance, taskInsurance, taskTaxDeduction, taskBonus, taskDeduction);
        var allowanceColumn = taskAllowance.Result;
        var insuranceColumn = taskInsurance.Result;
        var taxDeductionColumn = taskTaxDeduction.Result;
        var bonusColumn = taskBonus.Result;
        var deductionColumn = taskDeduction.Result;

        var res = new List<DynamicColumn>();
        try
        {
            //Allowances
            res.AddRange(allowanceColumn);

            //Bonus
            res.AddRange(bonusColumn);
            res.AddRange(new List<DynamicColumn>()
            {
                new DynamicColumn()
                {
                    Header = "Thưởng khác",
                    Field = FieldOtherBonus,
                }
            });

            //Overall
            res.Add(new DynamicColumn()
            {
                Header = "Thành tiền (Tổng thu nhập)",
                Field = FieldTotalIncome,
            });

            //BHXH
            res.AddRange(insuranceColumn);
            res.Add(new DynamicColumn()
            {
                Header = "Thành tiền (Tổng bảo hiểm)",
                Field = FieldBHXHCheckout,
            });


            //Tax Deduction
            res.AddRange(taxDeductionColumn);

            //Overall
            res.Add(new DynamicColumn()
            {
                Header = "Thành tiền (Tổng khấu trừ)",
                Field = FieldTotalTaxDeduction,
            });


            //Thuế TNCN
            res.AddRange(new List<DynamicColumn>()
            {
                new DynamicColumn()
                {
                    Header = "Các khoản miễn thuế",
                    Field = FieldNotInTax,
                },
                new DynamicColumn()
                {
                    Header = "Thu nhập chịu thuế",
                    Field = FieldTaxableIncome,
                },
                new DynamicColumn()
                {
                    Header = "Thu nhập tính thuế",
                    Field = FieldAssessableIncome,
                },
                new DynamicColumn()
                {
                    Header = "% Thuế xuất",
                    Field = FieldTaxRate,
                },
                new DynamicColumn()
                {
                    Header = "Thành tiền (TTNCN)",
                    Field = FieldTaxCheckout,
                }
            });

            //Advance
            res.Add(new DynamicColumn()
            {
                Header = "Tạm ứng",
                Field = FieldTotalAdvance,
            });

            //Deduction
            res.AddRange(deductionColumn);
            res.AddRange(new List<DynamicColumn>()
            {
                new DynamicColumn()
                {
                    Header = "Trừ khác",
                    Field = FieldOtherDeduction,
                },
                new DynamicColumn()
                {
                    Header = "Thành tiền (Tổng trừ k.thuế)",
                    Field = FieldTotalDeductionNoTax,
                },
            });

            //Phụ cấp không thuế
            res.AddRange(new List<DynamicColumn>()
            {
                new DynamicColumn()
                {
                    Header = "Ăn ca",
                    Field = FieldAllowanceLunch ,
                },
                new DynamicColumn()
                {
                    Header = "Thành tiền (Tổng cộng k.thuế)",
                    Field = FieldTotalBonusNoTax,
                },
            });

            //net
            res.Add(new DynamicColumn()
            {
                Header = "Thực lĩnh",
                Field = Fieldnet,
            });

            //Prefix
            res.InsertRange(0, new List<DynamicColumn>()
            {
                new DynamicColumn()
                {
                    Field = "employeeName",
                    Header = "Tên nhân viên"
                },
                new DynamicColumn()
                {
                    Field = "departmentName",
                    Header = "Phòng ban"
                },
                new DynamicColumn()
                {
                     Header = "Lương cơ bản",
                     Field = FieldBaseSalary,
                },
                new DynamicColumn()
                {
                     Header = "Số công",
                    Field = FieldRealHours,
                },
                new DynamicColumn()
                {
                    Header = "Thành tiền",
                    Field = FieldHourWageCheckout,
                },

            });

            for (int i = 0; i < res.Count; i++)
            {
                if (res[i].Field.Contains("PARAM_") || res[i].Field.Contains("PARAMS_") || res[i].Field.Contains("FORMULA_"))
                {
                    res[i].Field = fieldNamePrefix + res[i].Field;
                }
            }

        }
        catch (Exception e)
        {
            return new ApiResponse<IEnumerable<DynamicColumn>>()
            {
                IsSuccess = false,
                Metadata = new List<DynamicColumn>(),
                Message = new List<string>() { e.Message }

            };
        }

        return new ApiResponse<IEnumerable<DynamicColumn>>()
        {
            IsSuccess = true,
            Metadata = res
        };
    }
    public async Task<ApiResponse<IEnumerable<PayrollTableData>>> GetPayrollTableData(PayrollPeriod period,string dfrom, string dto, List<int> listEmployeeIds = null)
    {
        try
        {
            var payrollByPeriod = new List<Payroll>();
            if (listEmployeeIds != null)
            {
                payrollByPeriod = await _payrollRepository.GetAllQueryAble().Where(x => x.Year == period.Year && x.Month == period.Month && listEmployeeIds.Contains(x.EmployeeId)).ToListAsync();
            }
            else
            {
                payrollByPeriod = await _payrollRepository.GetAllQueryAble().Where(x => x.Year == period.Year && x.Month == period.Month).ToListAsync();
            }

            var lstContractIds = payrollByPeriod.Select(x => x.ContractId).ToList(); ;
            var lstEmployeeIds = payrollByPeriod.Select(x => x.EmployeeId).ToList(); ;
            var lstPayrollIds = payrollByPeriod.Select(x => x.Id).ToList();

            var taskAllowance = getListAllowanceInPeriod(lstContractIds);
            var taskInsurance = getListInsuranceInPeriod(lstContractIds);
            var taskTaxDeduction = getListTaxDeductionInPeriod(lstEmployeeIds);
            var taskBonus = getListBonusInPeriod(lstPayrollIds);
            var taskDeduction = getListDeductionInPeriod(lstPayrollIds);
            var taskWorkHours = _workShiftService.GetTotalHoursOfEmployeeWork(lstEmployeeIds, dfrom, dto);

            Task.WaitAll(taskAllowance, taskInsurance, taskTaxDeduction, taskBonus, taskDeduction, taskWorkHours);
            var allowanceColumn = taskAllowance.Result;
            var insuranceColumn = taskInsurance.Result;
            var taxDeductionColumn = taskTaxDeduction.Result;
            var bonusColumn = taskBonus.Result;
            var deductionColumn = taskDeduction.Result;
            var lstWorkHoursAPI = taskWorkHours.Result;

            var lstWorkHours = new List<TotalWorkHours>();
            if (lstWorkHoursAPI.IsSuccess)
            {
                lstWorkHours = lstWorkHoursAPI.Metadata.ToList();
            }

            #region + employee info
            var employeeInfo = (from e in _employeeRepository.GetAllQueryAble().Where(x => lstEmployeeIds.Contains(x.Id))
                                join c in _contractRepository.GetAllQueryAble().Where(x => lstContractIds.Contains(x.Id)) on e.ContractId equals c.Id into empGroup
                                from c in empGroup.DefaultIfEmpty()
                                join p in _positionRepository.GetAllQueryAble() on c.PositionId equals p.Id into contractGroup
                                from p in contractGroup.DefaultIfEmpty()
                                join d in _departmentRepository.GetAllQueryAble() on p.DepartmentId equals d.Id into positonGroup
                                from d in positonGroup.DefaultIfEmpty()
                                select new
                                {
                                    ContractId = c.Id,
                                    EmployeeName = c.Name,
                                    DepartmentName = d.Name,
                                    PositionName = p.Name,
                                    Email = e.Email,
                                    PhoneNumber = e.PhoneNumber,
                                    DateHired = c.StartDate.ToString("dd/MM/yyyy"),
                                    EmployeeId = "NV-00" + e.Id,

                                    BaseSalary = c.ContractSalary != null ? c.ContractSalary.BaseSalary : 0,
                                    BaseInsurance = c.ContractSalary != null ? c.ContractSalary.BaseInsurance : 0,
                                    RequiredDays = c.ContractSalary != null ? c.ContractSalary.RequiredDays : 0,
                                    RequiredHours = c.ContractSalary != null ? c.ContractSalary.RequiredHours : 0,
                                    WageDaily = c.ContractSalary != null ? c.ContractSalary.WageDaily : 0,
                                    WageHourly = c.ContractSalary != null ? c.ContractSalary.WageHourly : 0,
                                    Factor = c.ContractSalary != null ? c.ContractSalary.Factor : 0,
                                }).ToList();
            #endregion
            //list advance approved
            var lstAllowedAdvance = await _advanceRepository.GetAllQueryAble().Where(x => x.Month == period.Month && x.Year == period.Year && x.Status == AdvanceStatus.Approved).ToListAsync();

            //list tax rate
            var lstTaxRate = await _taxRateRepository.GetAllQueryAble().ToListAsync();

            //list formula
            var lstAllFormula = await _fomulaRepository.GetAllQueryAble().ToListAsync();

            var res = new List<PayrollTableData>();

            //Áp dụng công thức - ý tưởng: tính hết các parameter cố định, từ công thức truy về các parameter cố định và thực hiện phép tính

            foreach (var payroll in payrollByPeriod)
            {
                var tableRow = new PayrollTableData();
                var selectedEmployee = employeeInfo.FirstOrDefault(x => x.ContractId == payroll.ContractId);
                var selectedFormula = lstAllFormula.FirstOrDefault(x => x.Id == payroll.FomulaId);
                tableRow.PayrollId = payroll.Id;
                tableRow.EmployeeId = payroll.EmployeeId;
                tableRow.ContractId = payroll.ContractId;
                tableRow.EmployeeName = selectedEmployee?.EmployeeName ?? "Not found";
                tableRow.DepartmentName = selectedEmployee?.DepartmentName ?? "Not found";
                tableRow.PositionName = selectedEmployee?.PositionName ?? "Not found";
                tableRow.Email = selectedEmployee?.Email ?? "undefined";
                tableRow.PhoneNumber = selectedEmployee?.PhoneNumber ?? "undefined";
                tableRow.DateHired = selectedEmployee?.DateHired ?? "undefined";
                
                var realHours = lstWorkHours.FirstOrDefault(x=>x.EmployeeId==payroll.EmployeeId);

                double totalTax = 0;
                var stringFormulaAll = extractAllFormula(selectedFormula.Id, lstAllFormula);

                #region + Tổng thu nhập
                //lương thười gian
                tableRow.DynamicProperties[FieldBaseSalary] = selectedEmployee?.BaseSalary ?? 0;
                tableRow.DynamicProperties[FieldBaseHours] = selectedEmployee?.RequiredHours ?? 0;
                tableRow.DynamicProperties[FieldRealHours] = realHours!=null? realHours.TotalWorkedHours:0;
                tableRow.DynamicProperties[FieldBaseDays] = selectedEmployee?.RequiredDays ?? 0;
                tableRow.DynamicProperties[FieldRealDays] = realHours != null ? realHours.TotalWorkedHours/24 : 0;
                tableRow.DynamicProperties[FieldBaseWageDays] = selectedEmployee?.WageDaily ?? 0;
                tableRow.DynamicProperties[FieldBaseWageHours] = selectedEmployee?.WageHourly ?? 0;
                tableRow.DynamicProperties[FieldBaseFactor] = selectedEmployee?.Factor ?? 1;

                //Total allowance
                double totalAllowance = 0;
                foreach (var sc in allowanceColumn)
                {
                    //check if employee has sc
                    if (sc.ListIdBelongTo.Contains(payroll.ContractId)) tableRow.DynamicProperties[sc.Field] = sc.Amount;
                    else tableRow.DynamicProperties[sc.Field] = 0;
                    totalAllowance += tableRow.DynamicProperties[sc.Field];
                }
                tableRow.DynamicProperties[FieldTotalAllowance] = totalAllowance;


                //Bonus
                double totalBonus = 0;
                foreach (var sc in bonusColumn)
                {
                    //check if employee has sc
                    if (sc.ListIdBelongTo.Contains(payroll.Id)) tableRow.DynamicProperties[sc.Field] = sc.Amount;
                    else tableRow.DynamicProperties[sc.Field] = 0;
                    totalBonus += tableRow.DynamicProperties[sc.Field];
                }

                tableRow.DynamicProperties[FieldOtherBonus] = payroll.OtherBonus;
                totalBonus += tableRow.DynamicProperties[FieldOtherBonus];
                tableRow.DynamicProperties[FieldTotalBonus] = totalBonus;

                //TThành tiền lương thời gian
                var stringFormulaLCB = extractFormulaPartial(selectedFormula.Id, ConstantPartWageCheckout, lstAllFormula);
                if (stringFormulaLCB != null)
                {
                    tableRow.DynamicProperties[FieldHourWageCheckout] = calculateFormulaString(stringFormulaLCB, tableRow.DynamicProperties);
                    tableRow.DynamicProperties[FieldRealHours] = stringFormulaLCB.Contains(FieldRealDays) ?
                                                                tableRow.DynamicProperties[FieldRealDays] : tableRow.DynamicProperties[FieldRealHours];
                }
                else
                {
                    tableRow.DynamicProperties[FieldHourWageCheckout] =
                            tableRow.DynamicProperties[FieldBaseSalary] * tableRow.DynamicProperties[FieldRealHours] / (selectedEmployee?.RequiredHours ?? 0);

                }

                var stringFormulaTotalIncome = extractFormulaPartial(selectedFormula.Id, ConstantPartWageTotalIncome, lstAllFormula);
                if (stringFormulaTotalIncome != null)
                {
                    tableRow.DynamicProperties[FieldTotalIncome] = calculateFormulaString(stringFormulaTotalIncome, tableRow.DynamicProperties);
                }
                else
                {
                    tableRow.DynamicProperties[FieldTotalIncome] = tableRow.DynamicProperties[FieldTotalAllowance]
                                                                 + tableRow.DynamicProperties[FieldTotalBonus]
                                                                 + tableRow.DynamicProperties[FieldHourWageCheckout];
                }


                #endregion

                #region + Khấu trừ
                //Insurance
                double totalInsurance = 0;

                foreach (var sc in insuranceColumn)
                {
                    //check if employee has sc
                    if (sc.ListIdBelongTo.Contains(payroll.Id)) tableRow.DynamicProperties[sc.Field] = sc.Amount * (selectedEmployee?.BaseInsurance ?? 0);
                    else tableRow.DynamicProperties[sc.Field] = 0;
                    totalInsurance += tableRow.DynamicProperties[sc.Field];
                }
                tableRow.DynamicProperties[FieldBHXHCheckout] = totalInsurance;
                if (!stringFormulaAll.Contains(FieldBHXHCheckout)) tableRow.DynamicProperties[FieldBHXHCheckout] = 0;

                //Tax Deduction
                double totalTaxDeduction = 0;
                foreach (var sc in taxDeductionColumn)
                {
                    //check if employee has sc
                    if (sc.ListIdBelongTo.Contains(payroll.Id)) tableRow.DynamicProperties[sc.Field] = sc.Amount;
                    else tableRow.DynamicProperties[sc.Field] = 0;
                    totalTaxDeduction += tableRow.DynamicProperties[sc.Field];
                }
                tableRow.DynamicProperties[FieldTotalPersonalTaxDeduction] = totalTaxDeduction;


                //Thành tiền tổng khấu trừ
                tableRow.DynamicProperties[FieldTotalTaxDeduction] = tableRow.DynamicProperties[FieldBHXHCheckout] + tableRow.DynamicProperties[FieldTotalPersonalTaxDeduction];

                #endregion

                #region + Thuế TNCN
                //Các khoản miễn thuế
                tableRow.DynamicProperties[FieldNotInTax] = 0; //FIX

                //Thu nhập chịu thuế
                tableRow.DynamicProperties[FieldTaxableIncome] = tableRow.DynamicProperties[FieldTotalIncome] - tableRow.DynamicProperties[FieldNotInTax];

                //Thu nhập tính thuế
                tableRow.DynamicProperties[FieldAssessableIncome] = tableRow.DynamicProperties[FieldTaxableIncome] - tableRow.DynamicProperties[FieldTotalTaxDeduction];

                //Thuế suất
                var selectedBLT = lstTaxRate.FirstOrDefault(x => tableRow.DynamicProperties[FieldAssessableIncome] > x.MinTaxIncome
                                                                && tableRow.DynamicProperties[FieldAssessableIncome] < x.MaxTaxIncome);
                tableRow.DynamicProperties[FieldTaxRate] = selectedBLT?.Percent ?? 0;
                totalTax = tableRow.DynamicProperties[FieldAssessableIncome] * tableRow.DynamicProperties[FieldTaxRate] - selectedBLT?.MinusAmount ?? 0;
                tableRow.DynamicProperties[FieldTaxCheckout] = totalTax < 0 ? 0 : totalTax;

                #endregion

                #region + Các khoản không tính thuế

                //Advance
                var selectedAdvances = lstAllowedAdvance.Where(x => x.EmployeeId == payroll.EmployeeId).ToList();
                tableRow.DynamicProperties[FieldTotalAdvance] = selectedAdvances != null ? selectedAdvances.Select(x => x.Amount).Sum() : 0;

                //Deduction
                double totalDeductionNoTax = 0;
                foreach (var sc in deductionColumn)
                {
                    //check if employee has sc
                    if (sc.ListIdBelongTo.Contains(payroll.Id)) tableRow.DynamicProperties[sc.Field] = sc.Amount;
                    else tableRow.DynamicProperties[sc.Field] = 0;
                    totalDeductionNoTax += tableRow.DynamicProperties[sc.Field];
                }

                tableRow.DynamicProperties[FieldOtherDeduction] = payroll.OtherDeduction;
                totalDeductionNoTax += tableRow.DynamicProperties[FieldOtherDeduction];
                tableRow.DynamicProperties[FieldTotalDeductionNoTax] = totalDeductionNoTax;

                //Phụ cấp ko thuế
                double totalBonusNoTax = 0;
                totalBonusNoTax += tableRow.DynamicProperties[FieldAllowanceLunch] = 730000; //FIX
                tableRow.DynamicProperties[FieldTotalBonusNoTax] = totalBonusNoTax;

                #endregion


                //Thực lĩnh : app công thức

                if (stringFormulaAll != null)
                {
                    tableRow.DynamicProperties[Fieldnet] = calculateFormulaString(stringFormulaAll, tableRow.DynamicProperties);
                }
                else
                {
                    tableRow.DynamicProperties[Fieldnet] = tableRow.DynamicProperties[FieldTotalIncome] -
                                                       tableRow.DynamicProperties[FieldTaxCheckout] -
                                                       totalInsurance -
                                                       tableRow.DynamicProperties[FieldTotalAdvance] -
                                                       tableRow.DynamicProperties[FieldTotalDeductionNoTax] +
                                                       tableRow.DynamicProperties[FieldTotalBonusNoTax];
                }

                res.Add(tableRow);
            }


            return new ApiResponse<IEnumerable<PayrollTableData>>()
            {
                IsSuccess = true,
                Metadata = res.OrderBy(x => x.DepartmentName)
            };
        }
        catch (AggregateException ex)
        {
            return new ApiResponse<IEnumerable<PayrollTableData>>()
            {
                IsSuccess = false,
                Message = ex.InnerExceptions.Select(x => x.Message).ToList()
            };
        }
        catch (Exception e)
        {
            return new ApiResponse<IEnumerable<PayrollTableData>>()
            {
                IsSuccess = false,
                Message = new List<string>() { e.Message }
            };
        }
    }
    public async Task<ApiResponse<IEnumerable<PayrollTableData>>> GetPayrollTableData_OLD(PayrollPeriod period)
    {
        try
        {
            var payrollByPeriod = await _payrollRepository.GetAllQueryAble().Where(x => x.Year == period.Year && x.Month == period.Month).ToListAsync();
            var lstContractIds = payrollByPeriod.Select(x => x.ContractId).ToList(); ;
            var lstEmployeeIds = payrollByPeriod.Select(x => x.EmployeeId).ToList(); ;
            var lstPayrollIds = payrollByPeriod.Select(x => x.Id).ToList();

            var taskAllowance = getListAllowanceInPeriod(lstContractIds);
            var taskInsurance = getListInsuranceInPeriod(lstContractIds);
            var taskTaxDeduction = getListTaxDeductionInPeriod(lstEmployeeIds);
            var taskBonus = getListBonusInPeriod(lstPayrollIds);
            var taskDeduction = getListDeductionInPeriod(lstPayrollIds);

            Task.WaitAll(taskAllowance, taskInsurance, taskTaxDeduction, taskBonus, taskDeduction);
            var allowanceColumn = taskAllowance.Result;
            var insuranceColumn = taskInsurance.Result;
            var taxDeductionColumn = taskTaxDeduction.Result;
            var bonusColumn = taskBonus.Result;
            var deductionColumn = taskDeduction.Result;

            #region + employee info
            var employeeInfo = (from c in _contractRepository.GetAllQueryAble().Where(x => lstContractIds.Contains(x.Id))
                                join p in _positionRepository.GetAllQueryAble() on c.PositionId equals p.Id into contractGroup
                                from p in contractGroup.DefaultIfEmpty()
                                join d in _departmentRepository.GetAllQueryAble() on p.DepartmentId equals d.Id into positonGroup
                                from d in positonGroup.DefaultIfEmpty()
                                select new
                                {
                                    ContractId = c.Id,
                                    EmployeeName = c.Name,
                                    DepartmentName = d.Name,
                                    PositionName = p.Name,

                                    BaseSalary = c.ContractSalary != null ? c.ContractSalary.BaseSalary : 0,
                                    BaseInsurance = c.ContractSalary != null ? c.ContractSalary.BaseInsurance : 0,
                                    RequiredDays = c.ContractSalary != null ? c.ContractSalary.RequiredDays : 0,
                                    RequiredHours = c.ContractSalary != null ? c.ContractSalary.RequiredHours : 0,
                                    WageDaily = c.ContractSalary != null ? c.ContractSalary.WageDaily : 0,
                                    WageHourly = c.ContractSalary != null ? c.ContractSalary.WageHourly : 0,
                                    Factor = c.ContractSalary != null ? c.ContractSalary.Factor : 0,
                                }).ToList();
            #endregion
            //list advance approved
            var lstAllowedAdvance = await _advanceRepository.GetAllQueryAble().Where(x => x.Month == period.Month && x.Year == period.Year && x.Status == AdvanceStatus.Approved).ToListAsync();

            //list tax rate
            var lstTaxRate = _taxRateRepository.GetAllQueryAble().ToList();

            var res = new List<PayrollTableData>();

            //Áp dụng công thức - ý tưởng: tính hết các parameter cố định, từ công thức truy về các parameter cố định và thực hiện phép tính

            foreach (var payroll in payrollByPeriod)
            {
                var tableRow = new PayrollTableData();
                var selectedEmployee = employeeInfo.FirstOrDefault(x => x.ContractId == payroll.ContractId);
                tableRow.PayrollId = payroll.Id;
                tableRow.EmployeeId = payroll.EmployeeId;
                tableRow.ContractId = payroll.ContractId;
                tableRow.EmployeeName = selectedEmployee?.EmployeeName ?? "Not found";
                tableRow.DepartmentName = selectedEmployee?.DepartmentName ?? "Not found";
                tableRow.PositionName = selectedEmployee?.PositionName ?? "Not found";

                double totalIncome = 0;
                double totalInsurance = 0;
                double totalTaxDeduction = 0;
                double totalTax = 0;

                #region + Tổng thu nhập
                //lương thười gian
                tableRow.DynamicProperties[FieldBaseSalary] = selectedEmployee?.BaseSalary ?? 0;
                tableRow.DynamicProperties[FieldRealHours] = 13;
                tableRow.DynamicProperties[FieldHourWageCheckout] =
                    tableRow.DynamicProperties[FieldBaseSalary] * tableRow.DynamicProperties[FieldRealHours] / (selectedEmployee?.RequiredHours ?? 0);


                foreach (var sc in allowanceColumn)
                {
                    //check if employee has sc
                    if (sc.ListIdBelongTo.Contains(payroll.ContractId)) tableRow.DynamicProperties[sc.Field] = sc.Amount;
                    else tableRow.DynamicProperties[sc.Field] = 0;
                    totalIncome += tableRow.DynamicProperties[sc.Field];
                }

                //Bonus
                foreach (var sc in bonusColumn)
                {
                    //check if employee has sc
                    if (sc.ListIdBelongTo.Contains(payroll.Id)) tableRow.DynamicProperties[sc.Field] = sc.Amount;
                    else tableRow.DynamicProperties[sc.Field] = 0;
                    totalIncome += tableRow.DynamicProperties[sc.Field];
                }

                tableRow.DynamicProperties[FieldOtherBonus] = payroll.OtherBonus;
                totalIncome += tableRow.DynamicProperties[FieldOtherBonus];

                //Thành tiền tổng thu nhập
                tableRow.DynamicProperties[FieldTotalIncome] = totalIncome;
                #endregion

                #region + Khấu trừ
                //Insurance
                foreach (var sc in insuranceColumn)
                {
                    //check if employee has sc
                    if (sc.ListIdBelongTo.Contains(payroll.Id)) tableRow.DynamicProperties[sc.Field] = sc.Amount * (selectedEmployee?.BaseInsurance ?? 0);
                    else tableRow.DynamicProperties[sc.Field] = 0;
                    totalInsurance += tableRow.DynamicProperties[sc.Field];
                }
                tableRow.DynamicProperties[FieldBHXHCheckout] = totalInsurance;
                totalTaxDeduction += tableRow.DynamicProperties[FieldBHXHCheckout];

                //Tax Deduction
                foreach (var sc in taxDeductionColumn)
                {
                    //check if employee has sc
                    if (sc.ListIdBelongTo.Contains(payroll.Id)) tableRow.DynamicProperties[sc.Field] = sc.Amount;
                    else tableRow.DynamicProperties[sc.Field] = 0;
                    totalTaxDeduction += tableRow.DynamicProperties[sc.Field];
                }
                //Thành tiền tổng khấu trừ
                tableRow.DynamicProperties[FieldTotalTaxDeduction] = totalTaxDeduction;

                #endregion

                #region + Thuế TNCN
                //Các khoản miễn thuế
                tableRow.DynamicProperties[FieldNotInTax] = 0; //FIX

                //Thu nhập chịu thuế
                tableRow.DynamicProperties[FieldTaxableIncome] = tableRow.DynamicProperties[FieldTotalIncome] - tableRow.DynamicProperties[FieldNotInTax];

                //Thu nhập tính thuế
                tableRow.DynamicProperties[FieldAssessableIncome] = tableRow.DynamicProperties[FieldTaxableIncome] - tableRow.DynamicProperties[FieldTotalTaxDeduction];

                //Thuế suất
                var selectedBLT = lstTaxRate.FirstOrDefault(x => tableRow.DynamicProperties[FieldAssessableIncome] > x.MinTaxIncome
                                                                && tableRow.DynamicProperties[FieldAssessableIncome] < x.MaxTaxIncome);
                tableRow.DynamicProperties[FieldTaxRate] = selectedBLT?.Percent ?? 0;
                totalTax = tableRow.DynamicProperties[FieldAssessableIncome] * tableRow.DynamicProperties[FieldTaxRate] - selectedBLT?.MinusAmount ?? 0;
                tableRow.DynamicProperties[FieldTaxCheckout] = totalTax < 0 ? 0 : totalTax;
                #endregion

                #region + Các khoản không tính thuế

                //Advance
                var selectedAdvances = lstAllowedAdvance.Where(x => x.EmployeeId == payroll.EmployeeId).ToList();
                tableRow.DynamicProperties[FieldTotalAdvance] = selectedAdvances != null ? selectedAdvances.Select(x => x.Amount).Sum() : 0;

                //Deduction
                double totalDeductionNoTax = 0;
                foreach (var sc in deductionColumn)
                {
                    //check if employee has sc
                    if (sc.ListIdBelongTo.Contains(payroll.Id)) tableRow.DynamicProperties[sc.Field] = sc.Amount;
                    else tableRow.DynamicProperties[sc.Field] = 0;
                    totalDeductionNoTax += tableRow.DynamicProperties[sc.Field];
                }

                tableRow.DynamicProperties[FieldOtherDeduction] = payroll.OtherDeduction;
                totalDeductionNoTax += tableRow.DynamicProperties[FieldOtherDeduction];
                tableRow.DynamicProperties[FieldTotalDeductionNoTax] = totalDeductionNoTax;

                //Phụ cấp ko thuế
                double totalBonusNoTax = 0;
                totalBonusNoTax += tableRow.DynamicProperties[FieldAllowanceLunch] = 730000; //FIX
                tableRow.DynamicProperties[FieldTotalBonusNoTax] = totalBonusNoTax;

                #endregion

                //Thực lĩnh : app công thức
                tableRow.DynamicProperties[Fieldnet] = tableRow.DynamicProperties[FieldTotalIncome] -
                                                       tableRow.DynamicProperties[FieldTaxCheckout] -
                                                       totalInsurance -
                                                       tableRow.DynamicProperties[FieldTotalAdvance] -
                                                       tableRow.DynamicProperties[FieldTotalDeductionNoTax] +
                                                       tableRow.DynamicProperties[FieldTotalBonusNoTax];
                res.Add(tableRow);
            }


            return new ApiResponse<IEnumerable<PayrollTableData>>()
            {
                IsSuccess = true,
                Metadata = res.OrderBy(x => x.DepartmentName)
            };
        }
        catch (AggregateException ex)
        {
            return new ApiResponse<IEnumerable<PayrollTableData>>()
            {
                IsSuccess = false,
                Message = ex.InnerExceptions.Select(x => x.Message).ToList()
            };
        }
        catch (Exception e)
        {
            return new ApiResponse<IEnumerable<PayrollTableData>>()
            {
                IsSuccess = false,
                Message = new List<string>() { e.Message }
            };
        }
    }
    public double GetEmployeeSalary(int employeeId, PayrollPeriod period)
    {
        var selectedPayroll = _payrollRepository.GetAllQueryAble()
                                        .Where(x => x.EmployeeId == employeeId
                                               && x.Month == period.Month
                                               && x.Year == period.Year).FirstOrDefault();
        var selectedEmployee = _employeeRepository.GetAllQueryAble()
                                        .Where(x => x.Id == employeeId).FirstOrDefault();
        var selectedContract = _contractRepository.GetAllQueryAble().Where(x => x.Id == selectedEmployee.ContractId).FirstOrDefault();
        var selectedContractSalary = _contractSalaryRepository.GetAllQueryAble().Where(x => x.Id == selectedContract.ContractSalaryId).FirstOrDefault();

        double totalWageCheckout = 0;

        double totalBonus = 0;
        double totalDeduction = 0;

        double totalTaxDeduction = 0;
        double totalTax = 0;

        double totalAdvance = 0;

        double totalAllowance = 0;
        double totalInsurance = 0;

        //Income
        double realWageTime = 120;
        totalWageCheckout = selectedContractSalary.BaseSalary * realWageTime / selectedContractSalary.RequiredHours;

        var allBonusIds = _bonusDetailsRepository.GetAllQueryAble().Where(x => x.PayrollId == selectedPayroll.Id)?.Select(x => x.BonusId).ToList() ?? new List<int>();
        totalBonus = _bonusRepository.GetAllQueryAble().Where(x => allBonusIds.Contains(x.Id)).Sum(x => x.Amount);

        var allAllowanceIds = _contractAllowanceRepository.GetAllQueryAble().Where(x => x.ContractId == selectedContract.Id)?.Select(x => x.AllowanceId).ToList() ?? new List<int>();
        totalAdvance = _allowanceRepository.GetAllQueryAble().Where(x => allAllowanceIds.Contains(x.Id)).Sum(x => x.Amount);

        //Tax deduction


        // PIX

        //Other no tax
        var allDeductionIds = _deductionDetailsRepository.GetAllQueryAble().Where(x => x.PayrollId == selectedPayroll.Id)?.Select(x => x.DeductionId).ToList() ?? new List<int>();
        totalDeduction = _deductionRepository.GetAllQueryAble().Where(x => allDeductionIds.Contains(x.Id)).Sum(x => x.Amount);


        return 0;

    }

    //History
    public async Task<ApiResponse<bool>> SavePayrollHistory(PayrollHistoryModel payrollHistory)
    {
        try
        {
            var newPayrollHistory = new PayrollHistory()
            {
                Name = payrollHistory.Name,
                Month = payrollHistory.Month,
                Year = payrollHistory.Year,
                Note = payrollHistory.Note,
                PayrollHeader = JsonConvert.SerializeObject(payrollHistory.PayrollHeader),
                PayrollColumn = JsonConvert.SerializeObject(payrollHistory.PayrollColumn),
                PayrollData = JsonConvert.SerializeObject(payrollHistory.PayrollData),
                DisplayColumns = JsonConvert.SerializeObject(payrollHistory.DisplayColumns),
                CreatedAt = DateTime.Now
            };
            await _payrollHistoryRepository.AddAsync(newPayrollHistory);
            await _payrollHistoryRepository.SaveChangeAsync();
            return new ApiResponse<bool>()
            {
                IsSuccess = true,
                Metadata = true
            };
        }
        catch (Exception e)
        {
            return new ApiResponse<bool>()
            {
                IsSuccess = false,
                Metadata = false,
                Message = new List<string>() { e.Message }
            };
        }
    }
    public async Task<ApiResponse<List<PayrollHistoryModel>>> GetAllPayrollHistory()
    {
        try
        {
            var lstHistory = await _payrollHistoryRepository.GetAllQueryAble().Select(x => new PayrollHistoryModel()
            {
                Id = x.Id,
                Name = x.Name,
                Month = x.Month,
                Year = x.Year,
                Note = x.Note,
                CreatedAt = x.CreatedAt.ToString("dd/MM/yyyy") + " - " + x.CreatedAt.ToString("HH:mm"),
            }).ToListAsync();
            return new ApiResponse<List<PayrollHistoryModel>>()
            {
                IsSuccess = true,
                Metadata = lstHistory
            };
        }
        catch (Exception e)
        {
            return new ApiResponse<List<PayrollHistoryModel>>()
            {
                IsSuccess = false,
                Message = new List<string>() { e.Message }
            };
        }
    }
    public async Task<ApiResponse<PayrollHistoryModel>> GetPayrollHistoryDetails(int payrollHistoryId)
    {
        try
        {
            var lstHistoryDB = await _payrollHistoryRepository.GetAllQueryAble().FirstOrDefaultAsync(x => x.Id == payrollHistoryId);
            if (lstHistoryDB == null) throw new Exception("Không tìm thấy chi tiết bảng lưuong");

            var lstHistoryResult = new PayrollHistoryModel()
            {
                Id = lstHistoryDB.Id,
                Name = lstHistoryDB.Name,
                Month = lstHistoryDB.Month,
                Year = lstHistoryDB.Year,
                Note = lstHistoryDB.Note,
                CreatedAt = lstHistoryDB.CreatedAt.ToString("dd/MM/yyyy") + " - " + lstHistoryDB.CreatedAt.ToString("HH:mm"),
                PayrollHeader = JsonConvert.DeserializeObject<List<List<ColumnTableHeader>>>(lstHistoryDB.PayrollHeader),
                PayrollColumn = JsonConvert.DeserializeObject<List<DynamicColumn>>(lstHistoryDB.PayrollColumn),
                PayrollData = JsonConvert.DeserializeObject<List<PayrollTableData>>(lstHistoryDB.PayrollData),
                DisplayColumns = JsonConvert.DeserializeObject<List<DynamicColumn>>(lstHistoryDB.DisplayColumns),
            };
            return new ApiResponse<PayrollHistoryModel>()
            {
                IsSuccess = true,
                Metadata = lstHistoryResult
            };
        }
        catch (Exception e)
        {
            return new ApiResponse<PayrollHistoryModel>()
            {
                IsSuccess = false,
                Message = new List<string>() { e.Message }
            };
        }
    }
    public async Task<ApiResponse<bool>> RemovePayrollHistory(int id)
    {
        try
        {

            await _payrollHistoryRepository.RemoveAsync(id);
            await _payrollHistoryRepository.SaveChangeAsync();
            return new ApiResponse<bool> { IsSuccess = true };
        }
        catch (Exception ex)
        {
            return new ApiResponse<bool> { IsSuccess = false, Message = new List<string>() { ex.Message } };
        }
    }

    //Quy công thức về các paramerter base, fix
    // VD: fomula: ([PARAM_1]+[PARAM_2]-[FORMULA_4])*3-[FORMULA_5]
    private string extractAllFormula(int formulaId, List<Fomula> lstAllFotmulas)
    {
        var currFormula = lstAllFotmulas.FirstOrDefault(x => x.Id == formulaId);
        if (currFormula == null) return "";
        var fomulaDetails = currFormula.FomulaDetail;
        var lstParams = new List<string>();
        foreach (Match match in Regex.Matches(currFormula.FomulaDetail, _param_pattern_))
        {
            var param_partial = match.Groups[1].Value;
            if (param_partial.StartsWith("FORMULA_"))
            {
                var selectedFomula = lstAllFotmulas.FirstOrDefault(x => x.ParameterName == param_partial);
                if (selectedFomula != null)
                {
                    var fomula_extract = extractAllFormula(selectedFomula.Id, lstAllFotmulas);
                    fomulaDetails = fomulaDetails.Replace("[" + param_partial + "]", "(" + fomula_extract + ")");
                }

            }

        }
        return fomulaDetails;
    }
    private string extractFormulaPartial(int formulaId, string formulaFindPartString, List<Fomula> lstAllFotmulas, bool beginMerge = false)
    {
        var currFormula = lstAllFotmulas.FirstOrDefault(x => x.Id == formulaId);
        if (currFormula == null) return "";
        var fomulaDetails = "";
        if (beginMerge) fomulaDetails = currFormula.FomulaDetail;
        var lstParams = new List<string>();
        foreach (Match match in Regex.Matches(currFormula.FomulaDetail, _param_pattern_))
        {
            var param_partial = match.Groups[1].Value;
            if (param_partial.StartsWith("FORMULA_"))
            {
                var selectedFomula = lstAllFotmulas.FirstOrDefault(x => x.ParameterName == param_partial);

                if (selectedFomula != null)
                {
                    if (selectedFomula.ParameterName.Contains(formulaFindPartString))
                    {
                        return extractFormulaPartial(selectedFomula.Id, formulaFindPartString, lstAllFotmulas, true);

                    }
                    else if (beginMerge == true)
                    {
                        var fomula_extract = extractFormulaPartial(selectedFomula.Id, formulaFindPartString, lstAllFotmulas, true);
                        fomulaDetails = fomulaDetails.Replace("[" + param_partial + "]", "(" + fomula_extract + ")");
                    }
                    else if (fomulaDetails == "") fomulaDetails = extractFormulaPartial(selectedFomula.Id, formulaFindPartString, lstAllFotmulas, false);
                }
            }

        }

        return fomulaDetails;


    }
    private double calculateFormulaString(string fomulaString, Dictionary<string, double> map)
    {
        var formulasStringNumber = fomulaString;
        foreach (Match match in Regex.Matches(fomulaString, _param_pattern_))
        {
            var param_partial = match.Groups[1].Value;
            formulasStringNumber = formulasStringNumber.Replace("[" + param_partial + "]", map[param_partial].ToString());
        }
        var result = new DataTable().Compute(formulasStringNumber, null).ToString();
        return double.Parse(result);
    }
    public async Task<string> Test()
    {
        try
        {
            var bodyContentEmail = HandleFile.READ_FILE("Email", "Payslip.html")
                 .Replace("{sentDate}", DateTime.Now.ToString("dd/MM/yyyy"))
                 .Replace("{payPeriod}", "Tháng 11 năm 2024")
                 .Replace("{employeeName}", "Nguyễn Thành Hưng")
                 .Replace("{email}", "thanh.hung.st302@gmail.com")
                 .Replace("{employeeId}", "NVF-001")
                 .Replace("{dateHired}", "10/10/2020")
                 .Replace("{positionName}", "Kĩ sư phần mềm")
                 .Replace("{departmentName}", "Phòng IT")
                 .Replace("{taxCode}", "102-231-322-123")
                 .Replace("{" + $"{FieldTotalIncome}" + "}", "10000000")
                 .Replace("{" + $"{FieldBaseSalary}" + "}", "12000000")
                 .Replace("{" + $"{FieldBaseWageHours}" + "}", "100000")
                 .Replace("{" + $"{FieldBaseHours}" + "}", "600")
                 .Replace("{" + $"{FieldRealHours}" + "}", "400")
                 .Replace("{" + $"{FieldHourWageCheckout}" + "}", "24000000")
                 .Replace("{" + $"{FieldOtherBonus}" + "}", "400000")
                 .Replace("{" + $"{FieldTotalIncome}" + "}", "16000000")
                 .Replace("{" + $"{FieldTotalTaxDeduction}" + "}", "400000")
                 .Replace("{" + $"{FieldTaxCheckout}" + "}", "200000")
                 .Replace("{" + $"{FieldTotalAdvance}" + "}", "490000")
                 .Replace("{" + $"{FieldOtherDeduction}" + "}", "400")
                 .Replace("{" + $"{Fieldnet}" + "}", "40000000000");
            var bodyEmail = _emailService.TemplateContent
                            .Replace("<main>", "")
                            .Replace("/<main>", "")
                            .Replace("{content}", bodyContentEmail);

            var email = new Email()
            {
                To = "thanh.hung.st302@gmail.com",
                Body = bodyEmail,
                Subject = "[HRM TEST] - PHIẾU LƯƠNG THÁNG 11 NĂM 2024"
            };
            await _emailService.SendEmailToRecipient(email);
            return "Ok";
        }
        catch (Exception e)
        {
            return e.Message;
        }


    }


    #endregion


    #region +Get dynamic field allowances, insurance, bonus, deduction, tax deduction by period

    //allowance
    private async Task<List<DynamicColumn>> getListAllowanceInPeriod(List<int> lstContractIds)
    {
        var listAllSC = _contractAllowanceRepository.GetAllQueryAble().Where(x => lstContractIds.Contains(x.ContractId)).ToList();
        if (listAllSC == null || listAllSC.Count == 0) return new List<DynamicColumn>();
        var listUnionIds = new HashSet<int>(listAllSC.Select(x => x.AllowanceId).ToList());
        var allMapList = _allowanceRepository.GetAllQueryAble().Where(x => listUnionIds.Contains(x.Id)).ToList();
        return allMapList.Select(x => new DynamicColumn()
        {
            Id = x.Id,
            Header = x.Name,
            Field = x.ParameterName,
            Amount = x.Amount,
            ListIdBelongTo = listAllSC.Where(y => y.AllowanceId == x.Id).Select(x => x.ContractId).ToList()
        }).ToList();
    }

    //insurance
    private async Task<List<DynamicColumn>> getListInsuranceInPeriod(List<int> lstContractIds)
    {
        var listAllSC = _contractInsuranceRepository.GetAllQueryAble().Where(x => lstContractIds.Contains(x.ContractId)).ToList();
        if (listAllSC == null || listAllSC.Count == 0) return new List<DynamicColumn>();
        var listUnionIds = new HashSet<int>(listAllSC.Select(x => x.InsuranceId).ToList());
        var allMapList = _insuranceRepository.GetAllQueryAble().Where(x => listUnionIds.Contains(x.Id)).ToList();
        return allMapList.Select(x => new DynamicColumn()
        {
            Id = x.Id,
            Header = x.Name + " (" + x.PercentEmployee * 100 + "%)",
            Field = x.ParameterName,
            Amount = x.PercentEmployee,
            ListIdBelongTo = listAllSC.Where(y => y.InsuranceId == x.Id).Select(x => x.ContractId).ToList()
        }).ToList();
    }

    //tax deduction
    private async Task<List<DynamicColumn>> getListTaxDeductionInPeriod(List<int> lstEmployeeIds)
    {
        var listAllSC = _taxDeductionDetailsRepository.GetAllQueryAble().Where(x => lstEmployeeIds.Contains(x.EmployeeId)).ToList();
        if (listAllSC == null || listAllSC.Count == 0) return new List<DynamicColumn>();
        var listUnionIds = new HashSet<int>(listAllSC.Select(x => x.TaxDeductionId).ToList());
        var allMapList = _taxDeductionRepository.GetAllQueryAble().Where(x => listUnionIds.Contains(x.Id)).ToList();
        return allMapList.Select(x => new DynamicColumn()
        {
            Id = x.Id,
            Header = x.Name,
            Field = x.ParameterName,
            Amount = x.Amount,
            ListIdBelongTo = listAllSC.Where(y => y.TaxDeductionId == x.Id).Select(x => x.EmployeeId).ToList()
        }).ToList();
    }

    //bonus
    private async Task<List<DynamicColumn>> getListBonusInPeriod(List<int> lstPayrollIds)
    {
        var listAllSC = _bonusDetailsRepository.GetAllQueryAble().Where(x => lstPayrollIds.Contains(x.PayrollId)).ToList();
        if (listAllSC == null || listAllSC.Count == 0) return new List<DynamicColumn>();
        var listUnionIds = new HashSet<int>(listAllSC.Select(x => x.BonusId).ToList());
        var allMapList = _bonusRepository.GetAllQueryAble().Where(x => listUnionIds.Contains(x.Id)).ToList();
        return allMapList.Select(x => new DynamicColumn()
        {
            Id = x.Id,
            Header = x.Name,
            Field = x.ParameterName,
            Amount = x.Amount,
            ListIdBelongTo = listAllSC.Where(y => y.BonusId == x.Id).Select(x => x.PayrollId).ToList()
        }).ToList();
    }

    //deduction
    private async Task<List<DynamicColumn>> getListDeductionInPeriod(List<int> lstPayrollIds)
    {
        var listAllSC = _deductionDetailsRepository.GetAllQueryAble().Where(x => lstPayrollIds.Contains(x.PayrollId)).ToList();
        if (listAllSC == null || listAllSC.Count == 0) return new List<DynamicColumn>();
        var listUnionIds = new HashSet<int>(listAllSC.Select(x => x.DeductionId).ToList());
        var allMapList = _deductionRepository.GetAllQueryAble().Where(x => listUnionIds.Contains(x.Id)).ToList();
        return allMapList.Select(x => new DynamicColumn()
        {
            Id = x.Id,
            Header = x.Name,
            Field = x.ParameterName,
            Amount = x.Amount,
            ListIdBelongTo = listAllSC.Where(y => y.DeductionId == x.Id).Select(x => x.PayrollId).ToList()
        }).ToList();
    }

    #endregion


}



