using FluentValidation;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using Microsoft.EntityFrameworkCore;
using NPOI.POIFS.Properties;

namespace HRM.Services.Salary
{
    public interface ITaxRatesService
    {
        Task<ApiResponse<IEnumerable<TaxRateResult>>> GetAllTaxRate();
        Task<ApiResponse<bool>> AddNewTaxRate(TaxRateUpsert taxRateAdd);
        Task<ApiResponse<bool>> UpdateTaxRate(int id, TaxRateUpsert taxRateUpdate);
        Task<ApiResponse<bool>> RemoveTaxRate(int id);
    }
    public class TaxRatesService : ITaxRatesService
    {
        private readonly IBaseRepository<TaxRate> _baseRepository;
        private readonly IValidator<TaxRateUpsert> _taxRateUpsertValidator;
        public TaxRatesService(IBaseRepository<TaxRate> baseRepository,IValidator<TaxRateUpsert> taxRateUpsertValidator)
        {
            _baseRepository = baseRepository;
            _taxRateUpsertValidator = taxRateUpsertValidator;
        }
        public async Task<ApiResponse<IEnumerable<TaxRateResult>>> GetAllTaxRate()
        {
            try
            {
                return new ApiResponse<IEnumerable<TaxRateResult>>
                {
                    Metadata = await _baseRepository.GetAllQueryAble().Select(e => new TaxRateResult
                    {
                        Id = e.Id,
                        Name = e.Name,
                        Percent = e.Percent,
                        Condition = e.Condition,
                        MinTaxIncome = e.MinTaxIncome,
                        MaxTaxIncome = e.MaxTaxIncome,
                        MinusAmount = e.MinusAmount,
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
        public async Task<ApiResponse<bool>> AddNewTaxRate(TaxRateUpsert taxRateAdd)
        {
            try
            {
                var resultValidation = _taxRateUpsertValidator.Validate(taxRateAdd);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                await _baseRepository.AddAsync(new TaxRate { 
                    Name = taxRateAdd.Name.Trim(),
                    Percent = taxRateAdd.Percent,
                    Condition = taxRateAdd.Condition,
                    MinTaxIncome = taxRateAdd.MinTaxIncome,
                    MaxTaxIncome = taxRateAdd.MaxTaxIncome,
                    MinusAmount = taxRateAdd.MinusAmount,
                    ParameterName = taxRateAdd.ParameterName,
                });
                await _baseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> UpdateTaxRate(int id, TaxRateUpsert taxRateUpdate)
        {
            try
            {
                var resultValidation = _taxRateUpsertValidator.Validate(taxRateUpdate);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                var taxRate = await _baseRepository.GetAllQueryAble().Where(e => e.Id == id).FirstAsync();
                taxRate.Name = taxRateUpdate.Name.Trim();
                taxRate.ParameterName = taxRateUpdate.ParameterName.Trim();
                taxRate.Percent = taxRateUpdate.Percent;
                taxRate.Condition = taxRateUpdate.Condition;
                _baseRepository.Update(taxRate);
                await _baseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> RemoveTaxRate(int id)
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
