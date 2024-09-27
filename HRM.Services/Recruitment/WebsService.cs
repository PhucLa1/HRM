using AutoMapper;
using FluentValidation;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services.RecruitmentManager
{
    public interface IWebsService
    {
        Task<ApiResponse<IEnumerable<WebResult>>> GetAllWeb();
        Task<ApiResponse<bool>> AddNewWeb(WebUpsert webAdd);
        Task<ApiResponse<bool>> UpdateWeb(int id, WebUpsert webUpdate);
        Task<ApiResponse<bool>> RemoveWeb(int id);
    }
    public class WebsService : IWebsService
    {
        private readonly IBaseRepository<Web> _baseRepository;
        private readonly IValidator<WebUpsert> _webUpsertValidator;
        private readonly IMapper _mapper;
        public WebsService(
            IBaseRepository<Web> baseRepository,
            IValidator<WebUpsert> webUpsertValidator,
            IMapper mapper)
        {
            _baseRepository = baseRepository;
            _webUpsertValidator = webUpsertValidator;
            _mapper = mapper;
        }

        public async Task<ApiResponse<bool>> AddNewWeb(WebUpsert webAdd)
        {
            try
            {
                var resultValidation = _webUpsertValidator.Validate(webAdd);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                await _baseRepository.AddAsync(new Web { Name = webAdd.Name.Trim(), WebApi = webAdd.webApi.Trim() });
                await _baseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<IEnumerable<WebResult>>> GetAllWeb()
        {
            try
            {
                return new ApiResponse<IEnumerable<WebResult>>
                {

                    Metadata = _mapper.Map<IEnumerable<WebResult>>(await _baseRepository.GetAllQueryAble().ToListAsync()),
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<bool>> RemoveWeb(int id)
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

        public async Task<ApiResponse<bool>> UpdateWeb(int id, WebUpsert webUpdate)
        {
            try
            {
                var resultValidation = _webUpsertValidator.Validate(webUpdate);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                var postion = await _baseRepository.GetAllQueryAble().Where(e => e.Id == id).FirstAsync();
                postion.Name = webUpdate.Name.Trim();
                postion.WebApi = webUpdate.webApi.Trim();
				_baseRepository.Update(postion);
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
