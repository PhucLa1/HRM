using FluentValidation;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;

namespace HRM.Services.Salary
{
    public interface IAdvancesService
    {
        Task<ApiResponse<IEnumerable<AdvanceResult>>> GetAllAdvance();
        Task<ApiResponse<bool>> AddNewAdvance(AdvanceUpsert advanceAdd);
        Task<ApiResponse<bool>> UpdateAdvance(int id, AdvanceUpsert advanceUpdate);
        Task<ApiResponse<bool>> RemoveAdvance(int id);
    }
    public class AdvancesService : IAdvancesService
    {
        private readonly IBaseRepository<Advance> _advanceRepository;
        private readonly IValidator<AdvanceUpsert> _advanceUpsertValidator;
        private readonly IBaseRepository<Employee> _employeeRepository;
        private readonly IBaseRepository<Contract> _contractRepository;
        private readonly IBaseRepository<Department> _departmentRepository;
        private readonly IBaseRepository<Position> _positionRepository;
        public AdvancesService(IBaseRepository<Advance> advanceRepository,
                              IValidator<AdvanceUpsert> advanceUpsertValidator,
                              IBaseRepository<Employee> employeeRepository,
                              IBaseRepository<Contract> contractRepository,
                              IBaseRepository<Department> departmentRepository,
                              IBaseRepository<Position> positionRepository)
        {
            _advanceRepository = advanceRepository;
            _advanceUpsertValidator = advanceUpsertValidator;
            _employeeRepository = employeeRepository;
            _contractRepository = contractRepository;
            _departmentRepository = departmentRepository;
            _positionRepository = positionRepository;
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
                throw new Exception(ex.Message);
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
                await _advanceRepository.AddAsync(new Advance {
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
                throw new Exception(ex.Message);
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
                throw new Exception(ex.Message);
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
