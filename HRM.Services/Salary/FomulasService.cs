using FluentValidation;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services.Salary
{
    public interface IFomulasService
    {
        Task<ApiResponse<IEnumerable<FomulaResult>>> GetAllFomula();
        Task<ApiResponse<bool>> AddNewFomula(FomulaUpsert fomulaAdd);
        Task<ApiResponse<bool>> UpdateFomula(int id, FomulaUpsert fomulaUpdate);
        Task<ApiResponse<bool>> RemoveFomula(int id);
    }
    public class FomulasService : IFomulasService
    {
        private readonly IBaseRepository<Fomula> _baseRepository;
        private readonly IValidator<FomulaUpsert> _fomulaUpsertValidator;
        public FomulasService(IBaseRepository<Fomula> baseRepository,IValidator<FomulaUpsert> fomulaUpsertValidator)
        {
            _baseRepository = baseRepository;
            _fomulaUpsertValidator = fomulaUpsertValidator;
        }
        public async Task<ApiResponse<IEnumerable<FomulaResult>>> GetAllFomula()
        {
            try
            {
                return new ApiResponse<IEnumerable<FomulaResult>>
                {
                    Metadata = await _baseRepository.GetAllQueryAble().Select(e => new FomulaResult
                    {
                        Id = e.Id,
                        Name = e.Name,
                        FomulaDetail = e.FomulaDetail,
                        Note = e.Note
                    }).ToListAsync(),
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> AddNewFomula(FomulaUpsert fomulaAdd)
        {
            try
            {
                var resultValidation = _fomulaUpsertValidator.Validate(fomulaAdd);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                await _baseRepository.AddAsync(new Fomula { 
                    Name = fomulaAdd.Name.Trim(),
                    FomulaDetail = fomulaAdd.FomulaDetail,
                    Note = fomulaAdd.Note
                });
                await _baseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> UpdateFomula(int id, FomulaUpsert fomulaUpdate)
        {
            try
            {
                var resultValidation = _fomulaUpsertValidator.Validate(fomulaUpdate);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                var fomula = await _baseRepository.GetAllQueryAble().Where(e => e.Id == id).FirstAsync();
                fomula.Name = fomulaUpdate.Name.Trim();
                fomula.FomulaDetail = fomulaUpdate.FomulaDetail.Trim();
                fomula.Note = fomulaUpdate.Note;
                _baseRepository.Update(fomula);
                await _baseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> RemoveFomula(int id)
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
