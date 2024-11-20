using AutoMapper;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using FluentValidation;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using HRM.Repositories.Helper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRM.Services.Recruitment
{
	public interface IRecruitmentWebsService
	{
		Task<ApiResponse<IEnumerable<RecruitmentWebResult>>> GetAllRecruitmentWeb();
		Task<ApiResponse<bool>> AddNewRecruitmentWeb(RecruitmentWebUpsert recruitmentWebAdd);
		/*Task<ApiResponse<bool>> UpdateRecruitmentWeb(int id, RecruitmentWebUpsert recruitmentWebUpdate);*/
		Task<ApiResponse<bool>> RemoveRecruitmentWeb(int id);
	}
	public class RecruitmentWebsService : IRecruitmentWebsService
	{
		private readonly ILinkedInPostService _linkedInPostService;
		private readonly IBaseRepository<RecruitmentWeb> _baseRepository;
		private readonly IBaseRepository<JobPosting> _jobPostingRepository;
		private readonly IBaseRepository<Position> _positionRepository;
		private readonly IBaseRepository<Web> _webRepository;
		private readonly IBaseRepository<Employee> _employeeRepository;
		private readonly IBaseRepository<Contract> _contractRepository;
		private readonly IValidator<RecruitmentWebUpsert> _recruitmentWebUpsertValidator;
		private readonly IMapper _mapper;
		public RecruitmentWebsService(
			ILinkedInPostService linkedInPostService,
			IBaseRepository<RecruitmentWeb> baseRepository,
			IBaseRepository<JobPosting> jobPostingRepository,
			IBaseRepository<Web> webRepository,
			IBaseRepository<Position> positionRepository,
			IBaseRepository<Employee> employeeRepository,
			IBaseRepository<Contract> contractRepository,
		IValidator<RecruitmentWebUpsert> recruitmentWebUpsertValidator,
			IMapper mapper)
		{
			_linkedInPostService = linkedInPostService;
			_baseRepository = baseRepository;
			_jobPostingRepository = jobPostingRepository;
			_webRepository = webRepository;
			_positionRepository = positionRepository;
			_contractRepository = contractRepository;
			_employeeRepository = employeeRepository;
			_recruitmentWebUpsertValidator = recruitmentWebUpsertValidator;
			_mapper = mapper;
		}

		public async Task<ApiResponse<bool>> AddNewRecruitmentWeb(RecruitmentWebUpsert recruitmentWebAdd)
		{
			try
			{
				var jobpost = await (from jp in _jobPostingRepository.GetAllQueryAble()
										 where jp.Id == recruitmentWebAdd.JobPostingId
										 select jp).FirstOrDefaultAsync();
				var web = await (from w in _webRepository.GetAllQueryAble()
									 where w.Id == recruitmentWebAdd.WebId
									 select w).FirstOrDefaultAsync();
				var Mess = await (from jp in _jobPostingRepository.GetAllQueryAble()
									  join p in _positionRepository.GetAllQueryAble() on jp.PositionId equals p.Id into positionJoin
									  from p in positionJoin.DefaultIfEmpty() // Left join on positions
									  join e in _employeeRepository.GetAllQueryAble() on jp.EmployeeId equals e.Id into employeeJoin
									  from e in employeeJoin.DefaultIfEmpty()
									  join c in _contractRepository.GetAllQueryAble() on e.ContractId equals c.Id into contractJoin
									  from c in contractJoin.DefaultIfEmpty()
									  where jp.Id == recruitmentWebAdd.JobPostingId
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
									  }).FirstOrDefaultAsync();

				var resultValidation = _recruitmentWebUpsertValidator.Validate(recruitmentWebAdd);
				if (!resultValidation.IsValid)
				{
					return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
				}
				//var message = $"New job posting";
				//bool result = await _linkedInPostService.PostToLinkedIn(message, "AQUucvRfeLJHhB2D8jY-7e1S9f38J4P_ehFGzJ72N5A_fnj0BP1P6FhlC-aXMLTkHOR7bQ4u8axEvN-aUHp0HabEyDzFtb_0Hc_3yhKbvJYB_KhVjTb6loK0J7jN3wZOdxqtk7niHuFB-ZKf0wtDlHPi0yiRTiUCWeXBIcll5Hm7HbwKrOBrqJrjpY12salaYJRI_6eVO8uLDfP_KfT1fgk0nzt0XqBR-eAl11d7pcN17R8x9XdD9hKIAOVjx93DIAkXaunA1N5OfT5jWWBdsE-K1wZq70D_MXdO4PCCFgeZx1J-z4OgWsDYSKG8mal958J9avL3Ngprs_gCl2Ks1t6-BC9gAg");
				bool success = false;
				if (Mess != null)
				{
					var content = HandlePostForm.CreatePostContent(Mess);
					success = await _linkedInPostService.PostToLinkedIn4(content);
				}
				if (success == false) {
					return new ApiResponse<bool> { IsSuccess = false };
				}
				await _baseRepository.AddAsync(
						new RecruitmentWeb
						{
							JobPostingId = recruitmentWebAdd.JobPostingId,
							WebId = recruitmentWebAdd.WebId
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

		public async Task<ApiResponse<IEnumerable<RecruitmentWebResult>>> GetAllRecruitmentWeb()
		{
			try
			{
				return new ApiResponse<IEnumerable<RecruitmentWebResult>>
				{
					Metadata = await (from rw in _baseRepository.GetAllQueryAble()
									  join jp in _jobPostingRepository.GetAllQueryAble() on rw.JobPostingId equals jp.Id into jobPostingJoin
									  from jp in jobPostingJoin.DefaultIfEmpty() // Left join on jobposting
									  join w in _webRepository.GetAllQueryAble() on rw.WebId equals w.Id into webJoin
									  from w in webJoin.DefaultIfEmpty()
									  select new RecruitmentWebResult
									  {
										  Id = rw.Id,
										  WebId = rw.WebId,
										  Name = w.Name,
										  JobPostingId = rw.JobPostingId,
										  Description = jp.Description,
										  Location = jp.Location,
										  SalaryRangeMin = jp.SalaryRangeMin,
										  SalaryRangeMax = jp.SalaryRangeMax,
										  PostingDate = jp.PostingDate,
										  ExpirationDate = jp.ExpirationDate,
										  ExperienceRequired = jp.ExperienceRequired
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

		public async Task<ApiResponse<bool>> RemoveRecruitmentWeb(int id)
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

		/*public async Task<ApiResponse<bool>> UpdateRecruitmentWeb(int id, RecruitmentWebUpsert recruitmentWebUpdate)
		{
			throw new NotImplementedException();
		}*/
	}

}
