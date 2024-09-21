using FluentValidation;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services.Salary
{
    public interface IDeductionsService
    {
        Task<ApiResponse<IEnumerable<DeductionResult>>> GetAllDeduction();
        Task<ApiResponse<bool>> AddNewDeduction(DeductionUpsert deductionAdd);
        Task<ApiResponse<bool>> UpdateDeduction(int id, DeductionUpsert deductionUpdate);
        Task<ApiResponse<bool>> RemoveDeduction(int id);
    }
    public class DeductionsService : IDeductionsService
    {
        private readonly IBaseRepository<Deduction> _baseRepository;
        private readonly IValidator<DeductionUpsert> _deductionUpsertValidator;
        public DeductionsService(IBaseRepository<Deduction> baseRepository,IValidator<DeductionUpsert> deductionUpsertValidator)
        {
            _baseRepository = baseRepository;
            _deductionUpsertValidator = deductionUpsertValidator;
        }
        public async Task<ApiResponse<IEnumerable<DeductionResult>>> GetAllDeduction()
        {
            try
            {
                return new ApiResponse<IEnumerable<DeductionResult>>
                {
                    Metadata = await _baseRepository.GetAllQueryAble().Select(e => new DeductionResult
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Amount = e.Amount,
                        ParameterName = e.ParameterName
                    }).ToListAsync(),
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> AddNewDeduction(DeductionUpsert deductionAdd)
        {
            try
            {
                var resultValidation = _deductionUpsertValidator.Validate(deductionAdd);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                await _baseRepository.AddAsync(new Deduction { 
                    Name = deductionAdd.Name.Trim(),
                    ParameterName = deductionAdd.ParameterName,
                    Amount = deductionAdd.Amount
                });
                await _baseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> UpdateDeduction(int id, DeductionUpsert deductionUpdate)
        {
            try
            {
                var resultValidation = _deductionUpsertValidator.Validate(deductionUpdate);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                var deduction = await _baseRepository.GetAllQueryAble().Where(e => e.Id == id).FirstAsync();
                deduction.Name = deductionUpdate.Name.Trim();
                deduction.ParameterName = deductionUpdate.ParameterName.Trim();
                deduction.Amount = deductionUpdate.Amount;
                _baseRepository.Update(deduction);
                await _baseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> RemoveDeduction(int id)
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
    }
}
