using FluentValidation;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services.Salary
{
    public interface IAdvancesService
    {
        Task<ApiResponse<IEnumerable<AdvanceResult>>> GetAllAdvance();
        Task<ApiResponse<IEnumerable<AdvanceResult>>> GetAdvanceByEmployeeId(int employeeId);
        Task<ApiResponse<bool>> AddNewAdvance(AdvanceUpsert advanceAdd);
        Task<ApiResponse<bool>> UpdateAdvance(int id, AdvanceUpsert advanceUpdate);
        Task<ApiResponse<bool>> UpdateAdvanceStatus(int id, AdvanceStatus status);
        Task<ApiResponse<bool>> RemoveAdvance(int id);
    }
    public class AdvancesService : IAdvancesService
    {
        private readonly IBaseRepository<Advance> _advanceRepository;
        private readonly IValidator<AdvanceUpsert> _advanceUpsertValidator;
        private readonly IBaseRepository<Employee> _employeeRepository;
        private readonly IBaseRepository<Department> _departmentRepository;
        private readonly IBaseRepository<Contract> _contractRepository;
        private readonly IBaseRepository<ContractSalary> _contractSalaryRepository;
        public AdvancesService(IBaseRepository<Advance> advanceRepository,
                              IValidator<AdvanceUpsert> advanceUpsertValidator,
                              IBaseRepository<Employee> employeeRepository,
                              IBaseRepository<Department> departmentRepository,
                              IBaseRepository<Contract> contractRepository,
                              IBaseRepository<ContractSalary> contractSalaryRepository)
        {
            _advanceRepository = advanceRepository;
            _advanceUpsertValidator = advanceUpsertValidator;
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _contractRepository = contractRepository;
            _contractSalaryRepository = contractSalaryRepository;
        }
        public async Task<ApiResponse<IEnumerable<AdvanceResult>>> GetAllAdvance()
        {
            try
            {
                return new ApiResponse<IEnumerable<AdvanceResult>>
                {

                    Metadata = (from a in _advanceRepository.GetAllQueryAble()
                                join e in _employeeRepository.GetAllQueryAble() on a.EmployeeId equals e.Id into employeeGroup
                                from e in employeeGroup.DefaultIfEmpty() // Left join
                                join d in _departmentRepository.GetAllQueryAble() on e.Contract.Position.DepartmentId equals d.Id into departmentGroup
                                from d in departmentGroup.DefaultIfEmpty() // Left join
                                select new AdvanceResult
                                {
                                    Id = a.Id,
                                    Amount = a.Amount,
                                    Month = a.Month,
                                    Year = a.Year,
                                    PayPeriod = $"{(int)a.Month}/{a.Year}",
                                    EmployeeId = a.EmployeeId,
                                    Reason = a.Reason,
                                    Note = a.Note,
                                    Status = a.Status,
                                    EmployeeName = (e != null && e.Contract != null && e.Contract.Name != null) ? e.Contract.Name : "",
                                    DepartmentName = d != null ? d.Name : "",
                                    StatusName = getStatusName(a.Status)
                                }).ToList(),
                    IsSuccess = true

                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<AdvanceResult>>()
                {
                    IsSuccess = false,
                    Message = new List<string>() { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<IEnumerable<AdvanceResult>>> GetAdvanceByEmployeeId(int employeeId)
        {
            try
            {
                if (!_employeeRepository.GetAllQueryAble().Any(x => x.Id == employeeId))
                {
                    return new ApiResponse<IEnumerable<AdvanceResult>>()
                    {
                        IsSuccess = false,
                        Message = new List<string>() { "Không tìm thấy nhân viên tương ứng" }
                    };
                }
                return new ApiResponse<IEnumerable<AdvanceResult>>
                {

                    Metadata = (from a in _advanceRepository.GetAllQueryAble().Where(x => x.EmployeeId == employeeId)
                                join e in _employeeRepository.GetAllQueryAble() on a.EmployeeId equals e.Id into employeeGroup
                                from e in employeeGroup.DefaultIfEmpty() // Left join
                                join d in _departmentRepository.GetAllQueryAble() on e.Contract.Position.DepartmentId equals d.Id into departmentGroup
                                from d in departmentGroup.DefaultIfEmpty() // Left join
                                select new AdvanceResult
                                {
                                    Id = a.Id,
                                    Amount = a.Amount,
                                    Month = a.Month,
                                    Year = a.Year,
                                    PayPeriod = $"{(int)a.Month}/{a.Year}",
                                    EmployeeId = a.EmployeeId,
                                    Reason = a.Reason,
                                    Note = a.Note,
                                    Status = a.Status,
                                    EmployeeName = (e != null && e.Contract != null && e.Contract.Name != null) ? e.Contract.Name : "",
                                    DepartmentName = d != null ? d.Name : "",
                                    StatusName = getStatusName(a.Status)
                                }).ToList(),
                    IsSuccess = true

                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<IEnumerable<AdvanceResult>>()
                {
                    IsSuccess = false,
                    Message = new List<string>() { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<bool>> AddNewAdvance(AdvanceUpsert advanceAdd)
        {
            try
            {
                var resultValidation = _advanceUpsertValidator.Validate(advanceAdd);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                //check ứng lương quá 2 lần trong 1 tháng]
                var seletedEmployeePeriod = _advanceRepository.GetAllQueryAble()
                                                    .Where(x => x.EmployeeId == advanceAdd.EmployeeId
                                                            && x.Year == advanceAdd.Year && x.Month == advanceAdd.Month
                                                            && (x.Status == AdvanceStatus.Approved || x.Status == AdvanceStatus.Pending)).ToList();
                if (seletedEmployeePeriod.Count >= 2)
                {
                    throw new Exception("Nhân viên không được phép ứng lương quá 2 lần trong 1 kì lương");
                }
                var selectedEmployee = _employeeRepository.GetAllQueryAble().Where(x => x.Id == advanceAdd.EmployeeId).FirstOrDefault();
                if (selectedEmployee == null) throw new Exception("Nhân viên không tồn tại");
                var selectedContract = _contractRepository.GetAllQueryAble().Where(x => x.Id == selectedEmployee.Id).FirstOrDefault();
                if (selectedContract == null) throw new Exception("Hợp đồng đã hết hạn hoặc không tồn tại");
                var selectedContractSalary = _contractSalaryRepository.GetAllQueryAble().Where(x => x.Id == selectedContract.ContractSalaryId).FirstOrDefault();
                if (selectedContractSalary == null) throw new Exception("Nhân viên chưa được quy định lương trong hợp đồng");

                var curHourKeeping = 120;
                var currWage = selectedContractSalary.BaseSalary * curHourKeeping / selectedContractSalary.RequiredHours;
                if (seletedEmployeePeriod.Sum(x => x.Amount) + advanceAdd.Amount > currWage*0.7)
                {
                    throw new Exception("Số tiền lương ứng đã vượt mức quy định");
                }
                await _advanceRepository.AddAsync(new Advance
                {
                    Amount = advanceAdd.Amount,
                    Month = advanceAdd.Month,
                    Year = advanceAdd.Year,
                    EmployeeId = advanceAdd.EmployeeId,
                    Reason = advanceAdd.Reason.Trim(),
                    Note = advanceAdd.Note,
                    Status = advanceAdd.Status
                });
                await _advanceRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>()
                {
                    IsSuccess = false,
                    Message = new List<string>() { ex.Message }
                };
            }
        }
        public async Task<ApiResponse<bool>> UpdateAdvance(int id, AdvanceUpsert advanceUpdate)
        {
            try
            {
                var resultValidation = _advanceUpsertValidator.Validate(advanceUpdate);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                var advance = await _advanceRepository.GetAllQueryAble().Where(e => e.Id == id).FirstAsync();
                advance.Amount = advanceUpdate.Amount;
                advance.Month = advanceUpdate.Month;
                advance.Year = advanceUpdate.Year;
                advance.EmployeeId = advanceUpdate.EmployeeId;
                advance.Reason = advanceUpdate.Reason.Trim();
                advance.Note = advanceUpdate.Note;
                advance.Status = advanceUpdate.Status;
                _advanceRepository.Update(advance);
                await _advanceRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>()
                {
                    IsSuccess = false,
                    Message = new List<string>() { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<bool>> UpdateAdvanceStatus(int id, AdvanceStatus status)
        {
            try
            {
                var selectedAdvance = await _advanceRepository.GetAllQueryAble().Where(e => e.Id == id).FirstAsync();
                if (selectedAdvance == null)
                {
                    return new ApiResponse<bool> { IsSuccess = false, Message = new List<string>() { "Không tìm thấy yêu càu ứng lương!" } };

                }
                selectedAdvance.Status = status;
                _advanceRepository.Update(selectedAdvance);
                await _advanceRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                return new ApiResponse<bool>()
                {
                    IsSuccess = false,
                    Message = new List<string>() { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<bool>> RemoveAdvance(int id)
        {
            try
            {
                await _advanceRepository.RemoveAsync(id);
                await _advanceRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private static string getStatusName(AdvanceStatus status)
        {
            if (status == AdvanceStatus.Pending) return "Chờ duyệt";
            else if (status == AdvanceStatus.Approved) return "Đã duyệt";
            else return "Từ chối";
        }

    }
}
