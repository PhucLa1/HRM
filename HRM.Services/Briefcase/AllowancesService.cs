using AutoMapper;
using FluentValidation;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services.Briefcase
{
    public interface IAllowancesService
    {
        Task<ApiResponse<IEnumerable<AllowanceResult>>> GetAllAllowance();
        Task<ApiResponse<bool>> AddNewAllowance(AllowanceUpsert allowanceAdd);
        Task<ApiResponse<bool>> UpdateAllowance(int id, AllowanceUpsert allowanceUpdate);
        Task<ApiResponse<bool>> RemoveAllowance(int id);
    }
    public class AllowancesService : IAllowancesService
    {
        private readonly IBaseRepository<Allowance> _baseRepository;
        private readonly IValidator<AllowanceUpsert> _allowanceUpsertValidator;
        private readonly IMapper _mapper;
        public AllowancesService(
            IBaseRepository<Allowance> baseRepository,
            IValidator<AllowanceUpsert> allowanceUpsertValidator,
            IMapper mapper)
        {
            _baseRepository = baseRepository;
            _allowanceUpsertValidator = allowanceUpsertValidator;
            _mapper = mapper;
        }
        public async Task<ApiResponse<IEnumerable<AllowanceResult>>> GetAllAllowance()
        {
            try
            {
                return new ApiResponse<IEnumerable<AllowanceResult>>
                {
                    /*
                    Metadata = await _baseRepository.GetAllQueryAble().Select(e => new AllowanceResult
                    {
                        Id = e.Id,
                        Name = e.Name
                    }).ToListAsync(),
                    */
                    Metadata = _mapper.Map<IEnumerable<AllowanceResult>>(await _baseRepository.GetAllQueryAble().ToListAsync()),
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> AddNewAllowance(AllowanceUpsert allowanceAdd)
        {
            try
            {
                var resultValidation = _allowanceUpsertValidator.Validate(allowanceAdd);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                await _baseRepository.AddAsync(new Allowance { 
                    Name = allowanceAdd.Name.Trim(),
                    Amount = allowanceAdd.Amount,
                    Terms = allowanceAdd.Terms.Trim(),
                    ParameterName = allowanceAdd.ParameterName.Trim()
                });
                await _baseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> UpdateAllowance(int id, AllowanceUpsert allowanceUpdate)
        {
            try
            {
                var resultValidation = _allowanceUpsertValidator.Validate(allowanceUpdate);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                var allowance = await _baseRepository.GetAllQueryAble().Where(e => e.Id == id).FirstAsync();
                    allowance.Name = allowanceUpdate.Name.Trim();
                    allowance.Amount = allowanceUpdate.Amount;
                    allowance.Terms = allowanceUpdate.Terms.Trim();
                    allowance.ParameterName = allowanceUpdate.ParameterName.Trim();
                _baseRepository.Update(allowance);
                await _baseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> RemoveAllowance(int id)
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
