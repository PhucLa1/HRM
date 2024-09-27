using AutoMapper;
using FluentValidation;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services.RecruitmentManager
{
    public interface IJobsService
    {
        Task<ApiResponse<IEnumerable<JobResult>>> GetAllJob();
        Task<ApiResponse<bool>> AddNewJob(JobUpsert jobAdd);
        Task<ApiResponse<bool>> UpdateJob(int id, JobUpsert jobUpdate);
        Task<ApiResponse<bool>> RemoveJob(int id);
    }
    public class JobsService : IJobsService
    {
        private readonly IBaseRepository<Job> _baseRepository;
        private readonly IValidator<JobUpsert> _jobUpsertValidator;
        private readonly IMapper _mapper;
        public JobsService(
            IBaseRepository<Job> baseRepository,
            IValidator<JobUpsert> jobUpsertValidator,
            IMapper mapper)
        {
            _baseRepository = baseRepository;
            _jobUpsertValidator = jobUpsertValidator;
            _mapper = mapper;
        }
        public async Task<ApiResponse<IEnumerable<JobResult>>> GetAllJob()
        {
            try
            {
                return new ApiResponse<IEnumerable<JobResult>>
                {
                    Metadata = _mapper.Map<IEnumerable<JobResult>>(await _baseRepository.GetAllQueryAble().ToListAsync()),
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> AddNewJob(JobUpsert jobAdd)
        {
            try
            {
                var resultValidation = _jobUpsertValidator.Validate(jobAdd);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                await _baseRepository.AddAsync(new Job { Name = jobAdd.Name.Trim() });
                await _baseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> UpdateJob(int id, JobUpsert jobUpdate)
        {
            try
            {
                var resultValidation = _jobUpsertValidator.Validate(jobUpdate);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
                }
                var postion = await _baseRepository.GetAllQueryAble().Where(e => e.Id == id).FirstAsync();
                postion.Name = jobUpdate.Name.Trim();
                _baseRepository.Update(postion);
                await _baseRepository.SaveChangeAsync();
                return new ApiResponse<bool> { IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiResponse<bool>> RemoveJob(int id)
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
