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
        private readonly IBaseRepository<Advance> _baseRepository;
        private readonly IValidator<AdvanceUpsert> _advanceUpsertValidator;
        private readonly IBaseRepository<Employee> _employeeRepository;
        private readonly IBaseRepository<Contract> _contractRepository;
        public AdvancesService(IBaseRepository<Advance> baseRepository,
                              IValidator<AdvanceUpsert> advanceUpsertValidator,
                              IBaseRepository<Employee> employeeRepository,
                              IBaseRepository<Contract> contractRepository)
        {
            _baseRepository = baseRepository;
            _advanceUpsertValidator = advanceUpsertValidator;
            _employeeRepository = employeeRepository;
            _contractRepository = contractRepository;   
        }
        public async Task<ApiResponse<IEnumerable<AdvanceResult>>> GetAllAdvance()
        {
            try
            {
                return new ApiResponse<IEnumerable<AdvanceResult>>
                {

                    Metadata =  (from a in  _baseRepository.GetAllQueryAble()
                                join e in _employeeRepository.GetAllQueryAble() on a.EmployeeId equals e.Id
                                join c in _contractRepository.GetAllQueryAble() on e.ContractId equals c.Id
                                select new AdvanceResult
                                {
                                    Id = a.Id,
                                    Amount = a.Amount,
                                    PayPeriod = a.PayPeriod,
                                    EmployeeId = a.EmployeeId,
                                    Reason = a.Reason,
                                    Note = a.Note,
                                    Status = a.Status,
                                    EmployeeName = c.Name,
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
                await _baseRepository.AddAsync(new Advance {
                    Amount = advanceAdd.Amount,
                    PayPeriod = advanceAdd.PayPeriod,
                    EmployeeId = advanceAdd.EmployeeId,
                    Reason = advanceAdd.Reason.Trim(),
                    Note = advanceAdd.Note,
                    Status = advanceAdd.Status
                });
                await _baseRepository.SaveChangeAsync();
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
                var advance = await _baseRepository.GetAllQueryAble().Where(e => e.Id == id).FirstAsync();
                advance.Amount = advanceUpdate.Amount;
                advance.PayPeriod = advanceUpdate.PayPeriod;
                advance.EmployeeId = advanceUpdate.EmployeeId;
                advance.Reason = advanceUpdate.Reason.Trim();
                advance.Note = advanceUpdate.Note;
                advance.Status = advanceUpdate.Status;
                _baseRepository.Update(advance);
                await _baseRepository.SaveChangeAsync();
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
                await _baseRepository.RemoveAsync(id);
                await _baseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private string getStatusName(AdvanceStatus status)
        {
            if (status == AdvanceStatus.Pending) return "Chờ duyệt";
            else if (status == AdvanceStatus.Approved) return "Đã duyệt";
            else return "Từ chối";
        }
    }
}
