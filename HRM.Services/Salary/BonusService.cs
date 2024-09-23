using FluentValidation;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services.Salary
{
    public interface IBonusService
    {
        Task<ApiResponse<IEnumerable<BonusResult>>> GetAllBonus();
        Task<ApiResponse<bool>> AddNewBonus(BonusUpsert bonusAdd);
        Task<ApiResponse<bool>> UpdateBonus(int id, BonusUpsert bonusUpdate);
        Task<ApiResponse<bool>> RemoveBonus(int id);
    }
    public class BonusService : IBonusService
    {
        private readonly IBaseRepository<Bonus> _baseRepository;
        private readonly IValidator<BonusUpsert> _bonusUpsertValidator;
        public BonusService(IBaseRepository<Bonus> baseRepository,IValidator<BonusUpsert> bonusUpsertValidator)
        {
            _baseRepository = baseRepository;
            _bonusUpsertValidator = bonusUpsertValidator;
        }
        public async Task<ApiResponse<IEnumerable<BonusResult>>> GetAllBonus()
        {
            try
            {
                return new ApiResponse<IEnumerable<BonusResult>>
                {
                    Metadata = await _baseRepository.GetAllQueryAble().Select(e => new BonusResult
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
        public async Task<ApiResponse<bool>> AddNewBonus(BonusUpsert bonusAdd)
        {
            try
            {
                var resultValidation = _bonusUpsertValidator.Validate(bonusAdd);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                await _baseRepository.AddAsync(new Bonus { 
                    Name = bonusAdd.Name.Trim(),
                    ParameterName = bonusAdd.ParameterName,
                    Amount = bonusAdd.Amount
                });
                await _baseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> UpdateBonus(int id, BonusUpsert bonusUpdate)
        {
            try
            {
                var resultValidation = _bonusUpsertValidator.Validate(bonusUpdate);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                var bonus = await _baseRepository.GetAllQueryAble().Where(e => e.Id == id).FirstAsync();
                bonus.Name = bonusUpdate.Name.Trim();
                bonus.ParameterName = bonusUpdate.ParameterName.Trim();
                bonus.Amount = bonusUpdate.Amount;
                _baseRepository.Update(bonus);
                await _baseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> RemoveBonus(int id)
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
