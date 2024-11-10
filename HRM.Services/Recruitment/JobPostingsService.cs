using AutoMapper;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using DocumentFormat.OpenXml.Spreadsheet;
using FluentValidation;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Services.Recruitment
{
	public interface IJobPostingsService
	{
		Task<ApiResponse<IEnumerable<JobPostingResult>>> GetAllJobPosting();
		Task<ApiResponse<bool>> AddNewJobPosting(JobPostingUpsert jobPostingAdd);
		Task<ApiResponse<bool>> UpdateJobPosting(int id, JobPostingUpsert jobPostingUpdate);
		Task<ApiResponse<bool>> RemoveJobPosting(int id);
	}
	public class JobPostingsService : IJobPostingsService
	{
		private readonly IBaseRepository<JobPosting> _baseRepository;
		private readonly IBaseRepository<RecruitmentWeb> _recruitmentWebRepository;
		private readonly IBaseRepository<Employee> _employeeRepository;
		private readonly IBaseRepository<Position> _positionRepository;
		private readonly IBaseRepository<Contract> _contractRepository;
		private readonly IValidator<JobPostingUpsert> _jobPostingUpsertValidator;
		private readonly IMapper _mapper;
		public JobPostingsService(
			IBaseRepository<JobPosting> baseRepository,
			IBaseRepository<RecruitmentWeb> recruitmentWebRepository,
			IBaseRepository<Employee> employeeRepository,
			IBaseRepository<Position> positionRepository,
			IBaseRepository<Contract> contractRepository,
		IValidator<JobPostingUpsert> jobPostingUpsertValidator,
			IMapper mapper)
		{
			_baseRepository = baseRepository;
			_recruitmentWebRepository = recruitmentWebRepository;
			_employeeRepository = employeeRepository;
			_positionRepository = positionRepository;
			_contractRepository = contractRepository;
			_jobPostingUpsertValidator = jobPostingUpsertValidator;
			_mapper = mapper;
		}
		public async Task<ApiResponse<bool>> AddNewJobPosting(JobPostingUpsert jobPostingAdd)
		{
			try
			{
				var resultValidation = _jobPostingUpsertValidator.Validate(jobPostingAdd);
				if (!resultValidation.IsValid)
				{
					return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
				}
				await _baseRepository.AddAsync(
					new JobPosting
					{
						PositionId = jobPostingAdd.PositionId,
						Description = jobPostingAdd.Description,
						Location = jobPostingAdd.Location,
						SalaryRangeMin = jobPostingAdd.SalaryRangeMin,
						SalaryRangeMax = jobPostingAdd.SalaryRangeMax,
						PostingDate = jobPostingAdd.PostingDate,
						ExpirationDate = jobPostingAdd.ExpirationDate,
						ExperienceRequired = jobPostingAdd.ExperienceRequired,
						EmployeeId = jobPostingAdd.EmployeeId,
					}
				);
				await _baseRepository.SaveChangeAsync();
				return new ApiResponse<bool> { IsSuccess = true };
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<ApiResponse<IEnumerable<JobPostingResult>>> GetAllJobPosting()
		{
			try
			{
				return new ApiResponse<IEnumerable<JobPostingResult>>
				{
					Metadata = await (from jp in _baseRepository.GetAllQueryAble()
									  join p in _positionRepository.GetAllQueryAble() on jp.PositionId equals p.Id into positionJoin
									  from p in positionJoin.DefaultIfEmpty() // Left join on positions
									  join e in _employeeRepository.GetAllQueryAble() on jp.EmployeeId equals e.Id into employeeJoin
									  from e in employeeJoin.DefaultIfEmpty()
									  join c in _contractRepository.GetAllQueryAble() on e.ContractId equals c.Id into contractJoin
									  from c in contractJoin.DefaultIfEmpty()
									  select new JobPostingResult
									  {
										  Id = jp.Id,
										  PositionName = p.Name,
										  PositionId = jp.PositionId,
										  Description = jp.Description,
										  Location = jp.Location,
										  SalaryRangeMin = jp.SalaryRangeMin,
										  SalaryRangeMax = jp.SalaryRangeMax,
										  PostingDate = jp.PostingDate,
										  ExpirationDate = jp.ExpirationDate,
										  ExperienceRequired = jp.ExperienceRequired,
										  EmployeeName = c.Name,
										  EmployeeId = jp.EmployeeId
									  }).ToListAsync(),
					IsSuccess = true
				};
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
			throw new NotImplementedException();
		}

		public async Task<ApiResponse<bool>> RemoveJobPosting(int id)
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

		public async Task<ApiResponse<bool>> UpdateJobPosting(int id, JobPostingUpsert jobPostingUpdate)
		{
			try
			{
				var resultValidation = _jobPostingUpsertValidator.Validate(jobPostingUpdate);
				if (!resultValidation.IsValid)
				{
					return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
				}
				var job = await _baseRepository.GetAllQueryAble().Where(e => e.Id == id).FirstAsync();
				//Nhập lại dữ liệu
				job.PositionId = jobPostingUpdate.PositionId;
				job.Description = jobPostingUpdate.Description;
				job.Location = jobPostingUpdate.Location;
				job.SalaryRangeMin = jobPostingUpdate.SalaryRangeMin;
				job.SalaryRangeMax = jobPostingUpdate.SalaryRangeMax;
				job.PostingDate = jobPostingUpdate.PostingDate;
				job.ExpirationDate = jobPostingUpdate.ExpirationDate;
				job.ExperienceRequired = jobPostingUpdate.ExperienceRequired;
				job.EmployeeId = jobPostingUpdate.EmployeeId;

				//Lưu thông tin
				_baseRepository.Update(job);
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
