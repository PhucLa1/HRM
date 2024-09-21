using FluentValidation;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services.Salary
{
    public interface ITaxDeductionsService
    {
        Task<ApiResponse<IEnumerable<TaxDeductionResult>>> GetAllTaxDeduction();
        Task<ApiResponse<bool>> AddNewTaxDeduction(TaxDeductionUpsert taxDeductionAdd);
        Task<ApiResponse<bool>> UpdateTaxDeduction(int id, TaxDeductionUpsert taxDeductionUpdate);
        Task<ApiResponse<bool>> RemoveTaxDeduction(int id);
    }
    public class TaxDeductionsService : ITaxDeductionsService
    {
        private readonly IBaseRepository<TaxDeduction> _baseRepository;
        private readonly IValidator<TaxDeductionUpsert> _taxDeductionUpsertValidator;
        public TaxDeductionsService(IBaseRepository<TaxDeduction> baseRepository,IValidator<TaxDeductionUpsert> taxDeductionUpsertValidator)
        {
            _baseRepository = baseRepository;
            _taxDeductionUpsertValidator = taxDeductionUpsertValidator;
        }
        public async Task<ApiResponse<IEnumerable<TaxDeductionResult>>> GetAllTaxDeduction()
        {
            try
            {
                return new ApiResponse<IEnumerable<TaxDeductionResult>>
                {
                    Metadata = await _baseRepository.GetAllQueryAble().Select(e => new TaxDeductionResult
                    {
                        Id = e.Id,
                        Name = e.Name,
                        FomulaType = e.FomulaType,
                        Terms = e.Terms,
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
        public async Task<ApiResponse<bool>> AddNewTaxDeduction(TaxDeductionUpsert taxDeductionAdd)
        {
            try
            {
                var resultValidation = _taxDeductionUpsertValidator.Validate(taxDeductionAdd);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                await _baseRepository.AddAsync(new TaxDeduction { 
                    Name = taxDeductionAdd.Name.Trim(),
                    FomulaType = taxDeductionAdd.FomulaType,
                    Terms = taxDeductionAdd.Terms,
                    ParameterName = taxDeductionAdd.ParameterName,
                });
                await _baseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> UpdateTaxDeduction(int id, TaxDeductionUpsert taxDeductionUpdate)
        {
            try
            {
                var resultValidation = _taxDeductionUpsertValidator.Validate(taxDeductionUpdate);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                var taxDeduction = await _baseRepository.GetAllQueryAble().Where(e => e.Id == id).FirstAsync();
                taxDeduction.Name = taxDeductionUpdate.Name.Trim();
                taxDeduction.ParameterName = taxDeductionUpdate.ParameterName.Trim();
                taxDeduction.FomulaType = taxDeductionUpdate.FomulaType;
                taxDeduction.Terms = taxDeductionUpdate.Terms;
                _baseRepository.Update(taxDeduction);
                await _baseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> RemoveTaxDeduction(int id)
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
