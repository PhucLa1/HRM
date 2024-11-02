using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace HRM.Services.Salary;

public interface IPayrollsService
{
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

    //Payroll data
    Task<ApiResponse<List<List<ColumnTableHeader>>>> GetPayrollTableHeader(PayrollPeriod period);
    Task<ApiResponse<IEnumerable<DynamicColumn>>> GetPayrollTableColumn(PayrollPeriod period);
    Task<ApiResponse<IEnumerable<PayrollTableData>>> GetPayrollTableData(PayrollPeriod period);
}
public class PayrollsService : IPayrollsService
{
    private static string fieldNamePrefix = "dp.";
    private readonly string FieldBaseSalary = "PARAM_BASE_SALARY";
    private readonly string FieldRealHours = "PARAM_REAL_HOURS";
    private readonly string FieldHourWageCheckout = "PARAM_WAGE_HOURS";

    private readonly string FieldOtherBonus = "PARAM_OTHER_BONUS";
    private readonly string FieldTotalIncome = "FORMULA_TONG_THU_NHAP";

    private readonly string FieldBHXHCheckout = "PARAM_BHXH_CHECKOUT";
    private readonly string FieldTotalTaxDeduction = "FORMULA_TONG_KHAU_TRU";

    private readonly string FieldNotInTax = "PARAM_MIEN_THUE";
    private readonly string FieldTaxableIncome = "PARAM_TAXALBE_INCOME";
    private readonly string FieldAssessableIncome = "PARAM_ASSESSABLE_INCOME";
    private readonly string FieldTaxRate = "PARAM_TAX_RATE";
    private readonly string FieldTaxCheckout = "PARAM_TAX_CHECKOUT";
    

    private readonly string FieldTotalAdvance = "PARAM_TOTAL_ADVANCE";
    private readonly string FieldOtherDeduction = "PARAM_OTHER_DEDUCTION";
    private readonly string FieldTotalDeductionNoTax = "PARAM_TOTAL_DEDUCTION_NO_TAX";
    private readonly string FieldAllowanceLunch = "PARAM_ALLOWANCE_LUNCH_NO_TAX";
    private readonly string FieldTotalBonusNoTax = "PARAM_TOTAL_BONUS_NO_TAX";

    
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
                           IBaseRepository<TaxRate> taxRateRepository
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

    }

    #endregion

    #region + Add employee to payroll
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

            var departmentPosition = (from d in _departmentRepository.GetAllQueryAble()
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
                                      }).ToList();

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
        var employeeIdsOld = _payrollRepository.GetAllQueryAble().Where(x => x.Month == period.Month&&x.Year==period.Year).Select(x => x.EmployeeId).ToList();
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
                            FomulaId = 6
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

                allowanceResult = (from a in _allowanceRepository.GetAllQueryAble()
                                   join ca in _contractAllowanceRepository.GetAllQueryAble().Where(x => x.ContractId == selectedContract.Id) on a.Id equals ca.AllowanceId
                                   select new AllowanceResult()
                                   {
                                       Id = a.Id,
                                       Name = a.Name,
                                       Amount = a.Amount
                                   }).ToList();

                insuranceResult = (from i in _insuranceRepository.GetAllQueryAble()
                                   join ci in _contractInsuranceRepository.GetAllQueryAble().Where(x => x.ContractId == selectedContract.Id) on i.Id equals ci.InsuranceId
                                   select new InsuranceResult()
                                   {
                                       Id = i.Id,
                                       PercentEmployee = i.PercentEmployee,
                                       PercentCompany = i.PercentCompany
                                   }).ToList();

            }
            if (selectedEmployee != null)
            {
                if (selectedEmployee.taxDeductionDetails != null)
                {
                    taxDeduction = (from td in _taxDeductionRepository.GetAllQueryAble()
                                    join etd in selectedEmployee.taxDeductionDetails on td.Id equals etd.TaxDeductionId
                                    select new TaxDeductionResult()
                                    {
                                        Id = td.Id,
                                        Name = td.Name,
                                        Amount = td.Amount
                                    }).ToList();
                }
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
                ListBonusIds = (selectedPayroll.bonusDetails != null) ? selectedPayroll.bonusDetails.Select(x => x.BonusId).ToList() : new List<int>(),
                ListDeductionIds = (selectedPayroll.DeductionDetails != null) ? selectedPayroll.DeductionDetails.Select(x => x.DeductionId).ToList() : new List<int>(),
                ContractSalary = contractSalaryResult,
                ListAllowance = allowanceResult,
                ListInsurance = insuranceResult,
                ListTaxDeduction = taxDeduction
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
            var selectedListPayrollOld = _payrollRepository.GetAllQueryAble().Where(x => lstPayrollIds.Contains(x.Id)).ToList();
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
                Message = new List<string>() { e.Message}
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
                var listOldDetails = _bonusDetailsRepository.GetAllQueryAble().Where(x => lstPayrollIds.Contains(x.PayrollId)).ToList();
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
                var listOldDetails = _deductionDetailsRepository.GetAllQueryAble().Where(x => lstPayrollIds.Contains(x.PayrollId)).ToList();
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
            var selectedListPayrollOld = _payrollRepository.GetAllQueryAble().Where(x => lstPayrollIds.Contains(x.Id)).ToList();
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
                res = _bonusRepository.GetAllQueryAble().Select(x => new DynamicColumn()
                {
                    Field = x.Id.ToString(),
                    Header = $"{x.Name} ({x.Amount})",
                }).ToList();
            }
            else if (SCtype == 1) // Deduction
            {
                res = _deductionRepository.GetAllQueryAble().Select(x => new DynamicColumn()
                {
                    Field = x.Id.ToString(),
                    Header = $"{x.Name} ({x.Amount})",
                }).ToList();
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
            ListParentIds = new List<int>() { (int)ColId.ColIdSCIncome},
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
            RowSpan = insuranceColumn.Count==0?2:1
        });

        if (taxDeductionColumn.Count >0)
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
        bottomColumn.Add(new ColumnTableHeader() { Header = "Thưởng khác",Field = FieldOtherBonus, ListParentIds = new List<int>() { (int)ColId.ColIdSCIncome, (int)ColId.ColIdAllBonus }, });

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
        bottomColumn.Add(new ColumnTableHeader() { Header = "Trừ khác", Field = FieldOtherDeduction,ListParentIds = new List<int>() { (int)ColId.ColIdSCNotax, (int)ColId.ColIdAllDeductionNoTax }});
        bottomColumn.Add(new ColumnTableHeader() { Header = "Thành tiền", Field = FieldTotalDeductionNoTax, ListParentIds = new List<int>() { (int)ColId.ColIdSCNotax, (int)ColId.ColIdAllDeductionNoTax }  });

        // Bonus No tax
        bottomColumn.Add(new ColumnTableHeader(){Header = "Ăn ca",Field = FieldAllowanceLunch, ListParentIds = new List<int>() { (int)ColId.ColIdSCNotax, (int)ColId.ColIdAllBonusNoTax } });
        bottomColumn.Add(new ColumnTableHeader() { Header = "Thành tiền", Field = FieldTotalBonusNoTax, ListParentIds = new List<int>() { (int)ColId.ColIdSCNotax, (int)ColId.ColIdAllBonusNoTax } });
        res.Add(bottomColumn);

        for (int i = 0; i < res.Count; i++)
        {

            for (int j = 0; j < res[i].Count; j++)
            {
                if (i == res.Count-1)
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
                if (res[i].Field.Contains("PARAM_") || res[i].Field.Contains("PARAMS_") || res[i].Field.Contains("FORMULA_") )
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

    public async Task<ApiResponse<IEnumerable<PayrollTableData>>> GetPayrollTableData(PayrollPeriod period)
    {
        try
        {
            var payrollByPeriod = _payrollRepository.GetAllQueryAble().Where(x => x.Year == period.Year && x.Month == period.Month);
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
            var lstAllowedAdvance = _advanceRepository.GetAllQueryAble().Where(x => x.Month == period.Month && x.Year == period.Year && x.Status == AdvanceStatus.Approved).ToList();

            //list tax rate
            var lstTaxRate = _taxRateRepository.GetAllQueryAble().ToList();

            var res = new List<PayrollTableData>();


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
                    if (sc.ListIdBelongTo.Contains(payroll.Id)) tableRow.DynamicProperties[sc.Field] = sc.Amount* (selectedEmployee?.BaseInsurance ?? 0);
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
                tableRow.DynamicProperties[FieldTaxCheckout] = totalTax<0?0:totalTax;
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
    #endregion


    #region +Get dynamic field allowances, insurance, bonus, deduction, tax deduction by period

    //allowance
    private async Task<List<DynamicColumn>> getListAllowanceInPeriod(List<int> lstContractIds)
    {
        var listAllSC = _contractAllowanceRepository.GetAllQueryAble().Where(x => lstContractIds.Contains(x.ContractId)).ToList();
        if (listAllSC == null || listAllSC.Count == 0) return new List<DynamicColumn>();
        var listUnionIds = new HashSet<int>(listAllSC.Select(x => x.AllowanceId).ToList());
        var allMapList = _allowanceRepository.GetAllQueryAble().Where(x => listUnionIds.Contains(x.Id)).ToList();
        return allMapList.Select(x=>new DynamicColumn()
        {
            Id = x.Id,
            Header = x.Name,
            Field = x.ParameterName,
            Amount = x.Amount,
            ListIdBelongTo = listAllSC.Where(y=>y.AllowanceId == x.Id).Select(x=>x.ContractId).ToList()
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
            Header = x.Name+ " (" +x.PercentEmployee*100+"%)",
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
            Field =  x.ParameterName,
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



