using AutoMapper;
using FluentValidation;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services.Briefcase
{
    public interface IInsurancesService
    {
        Task<ApiResponse<IEnumerable<InsuranceResult>>> GetAllInsurance();
        Task<ApiResponse<bool>> AddNewInsurance(InsuranceUpsert insuranceAdd);
        Task<ApiResponse<bool>> UpdateInsurance(int id, InsuranceUpsert insuranceUpdate);
        Task<ApiResponse<bool>> RemoveInsurance(int id);
    }
    public class InsurancesService : IInsurancesService
    {
        private readonly IBaseRepository<Insurance> _baseRepository;
        private readonly IValidator<InsuranceUpsert> _insuranceUpsertValidator;
        private readonly IMapper _mapper;
        public InsurancesService(
            IBaseRepository<Insurance> baseRepository,
            IValidator<InsuranceUpsert> insuranceUpsertValidator,
            IMapper mapper)
        {
            _baseRepository = baseRepository;
            _insuranceUpsertValidator = insuranceUpsertValidator;
            _mapper = mapper;
        }
        public async Task<ApiResponse<IEnumerable<InsuranceResult>>> GetAllInsurance()
        {
            try
            {
                return new ApiResponse<IEnumerable<InsuranceResult>>
                {
                    /*
                    Metadata = await _baseRepository.GetAllQueryAble().Select(e => new InsuranceResult
                    {
                        Id = e.Id,
                        Name = e.Name
                    }).ToListAsync(),
                    */
                    Metadata = _mapper.Map<IEnumerable<InsuranceResult>>(await _baseRepository.GetAllQueryAble().ToListAsync()),
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> AddNewInsurance(InsuranceUpsert insuranceAdd)
        {
            try
            {
                var resultValidation = _insuranceUpsertValidator.Validate(insuranceAdd);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                await _baseRepository.AddAsync(new Insurance { 
                    Name = insuranceAdd.Name.Trim(),
                    PercentEmployee = insuranceAdd.PercentEmployee,
                    PercentCompany = insuranceAdd.PercentCompany,
                    ParameterName = insuranceAdd.ParameterName.Trim()
                });
                await _baseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> UpdateInsurance(int id, InsuranceUpsert insuranceUpdate)
        {
            try
            {
                var resultValidation = _insuranceUpsertValidator.Validate(insuranceUpdate);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                var insurance = await _baseRepository.GetAllQueryAble().Where(e => e.Id == id).FirstAsync();
                insurance.Name = insuranceUpdate.Name.Trim();
                insurance.PercentEmployee = insuranceUpdate.PercentEmployee;
                insurance.PercentCompany = insuranceUpdate.PercentCompany;
                insurance.ParameterName = insuranceUpdate.ParameterName.Trim();
                _baseRepository.Update(insurance);
                await _baseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> RemoveInsurance(int id)
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
