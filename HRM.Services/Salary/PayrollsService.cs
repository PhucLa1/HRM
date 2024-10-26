using DocumentFormat.OpenXml.InkML;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Results;

namespace HRM.Services.Salary
{
    public interface IPayrollsService
    {
        Task<ApiResponse<IEnumerable<TreeNode>>> GetCompanyTree();
    }
    public class PayrollsService : IPayrollsService
    {
        private readonly IBaseRepository<Payroll> _payrollRepository;
        private readonly IBaseRepository<Department> _departmentRepository;
        private readonly IBaseRepository<Position> _positionRepository;
        private readonly IBaseRepository<Contract> _contractRepository;
        private readonly IBaseRepository<Employee> _employeeRepository;
        public PayrollsService(IBaseRepository<Payroll> payrollRepository,
                               IBaseRepository<Department> departmentRepository,
                               IBaseRepository<Position> positionRepository,
                               IBaseRepository<Contract> contractRepository,
                               IBaseRepository<Employee> employeeRepository)
        {
            _payrollRepository = payrollRepository;
            _departmentRepository = departmentRepository;
            _positionRepository = positionRepository;
            _contractRepository = contractRepository;
            _employeeRepository = employeeRepository;
        }

        // Add employee to payroll
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

        public async Task<ApiResponse<bool>> UpdatePayroll(string period, List<int> employeeIds)
        {
            var employeeIdsOld = _payrollRepository.GetAllQueryAble().Where(x=>x.PayPeriod==period).Select(x => x.EmployeeId).ToList();
            var removedIds = employeeIds.Except(employeeIdsOld);
            var addedIds = employeeIdsOld.Except(employeeIds);
            using (var transaction = await _payrollRepository.Context.Database.BeginTransactionAsync())
            {

                try
                {
                    if (removedIds.Count() > 0)
                    {
                        var selectedPayroll = _payrollRepository.GetAllQueryAble().Where(x => x.PayPeriod == period && removedIds.Contains(x.EmployeeId));
                        if (selectedPayroll != null && selectedPayroll.Count() > 0)
                        {
                            _payrollRepository.RemoveRange(selectedPayroll);
                        }
                    }
                    if (addedIds.Count() > 0)
                    {
                        var listSelectedEmployee = _employeeRepository.GetAllQueryAble().Where(x => addedIds.Contains(x.Id));
                        var listInsertPayroll = new List<Payroll>();
                        foreach (var id in addedIds)
                        {
                            var selectedEmployee = listSelectedEmployee.FirstOrDefault(x => x.Id == id);
                            if (selectedEmployee == null) continue;
                            var payroll = new Payroll()
                            {
                                EmployeeId = id,
                                PayPeriod = period,
                                ContractId = selectedEmployee.ContractId,
                                OtherDeduction = 0,
                                OtherBonus = 0
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
                        Message=new List<string>() { ex.Message},
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
    
        public ApiResponse<List<int>> GetListEmployeeIdByPeriod(string period)
        {
            var employeeIds = _payrollRepository.GetAllQueryAble().Where(x => x.PayPeriod == period).Select(x => x.EmployeeId).ToList();
            return new ApiResponse<List<int>>()
            {
                IsSuccess = true,
                Metadata = employeeIds ?? new List<int>()
            };
        }


        // Update payroll of employee: bonus, deductions, fomula, other bonus, deductions


        // Update bonus, deductions, fomula for many employees


    }
}
