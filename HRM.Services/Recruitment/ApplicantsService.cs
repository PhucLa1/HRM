using AutoMapper;
using Azure.Core;
using FluentValidation;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using HRM.Repositories.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace HRM.Services.Recruitment
{
	public interface IApplicantsService
	{
		Task<ApiResponse<IEnumerable<ApplicantResult>>> GetAllApplicant();
		Task<ApiResponse<bool>> AddNewApplicant(ApplicantUpsert applicantAdd);
		Task<ApiResponse<bool>> UpdateApplicant(int id, ApplicantUpsert applicantUpdate);
		Task<ApiResponse<bool>> RemoveApplicant(int id);
	}
	public class ApplicantsService : IApplicantsService
	{
		private readonly IBaseRepository<Applicants> _baseRepository;
		private readonly IBaseRepository<Test> _testRepository;
		private readonly IBaseRepository<Position> _positionRepository;
		private readonly IValidator<ApplicantUpsert> _applicantUpsertValidator;
		private readonly IMapper _mapper;
		public ApplicantsService(
			IBaseRepository<Applicants> baseRepository,
			IValidator<ApplicantUpsert> applicantUpsertValidator,
			IBaseRepository<Test> testRepository,
			IBaseRepository<Position> positionRepository,
			IMapper mapper)
		{
			_baseRepository = baseRepository;
			_applicantUpsertValidator = applicantUpsertValidator;
			_positionRepository = positionRepository;
			_testRepository = testRepository;
			_mapper = mapper;
		}
		public async Task<ApiResponse<bool>> AddNewApplicant(ApplicantUpsert applicantAdd)
		{
			try
			{
				var resultValidation = _applicantUpsertValidator.Validate(applicantAdd);
				if (!resultValidation.IsValid)
				{
					return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
				}
				var applicant = new Applicants
				{
					Name = applicantAdd.Name.Trim(),
					Email = applicantAdd.Email,
					PhoneNumber = applicantAdd.Phone,
					FileDataUrl = "",
					PositionId = applicantAdd.PositionId,
					Rate = applicantAdd.Rate,
					TestId = applicantAdd.TestId,
					InterviewerName = applicantAdd.InterviewerName
				};
				if (applicantAdd.file.Length > 0)
				{
					string folder = "wwwroot/CV"; // Target folder for CV uploads in wwwroot
					applicant.FileDataUrl = HandleFile.UPLOAD_GETPATH(folder, applicantAdd.file);
				}

				await _baseRepository.AddAsync(applicant);
				await _baseRepository.SaveChangeAsync();
				return new ApiResponse<bool> { IsSuccess = true };
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<ApiResponse<IEnumerable<ApplicantResult>>> GetAllApplicant()
		{
			string url = "https://localhost:7025/";
			try
			{
				return new ApiResponse<IEnumerable<ApplicantResult>>
				{
					Metadata = await (from a in _baseRepository.GetAllQueryAble()
									  join p in _positionRepository.GetAllQueryAble() on a.PositionId equals p.Id into positionJoin
									  from p in positionJoin.DefaultIfEmpty()
									  join t in _testRepository.GetAllQueryAble() on a.TestId equals t.Id into testJoin
									  from t in testJoin.DefaultIfEmpty()
									  select new ApplicantResult
									  {
										  Id = a.Id,
										  Name = a.Name,
										  Email = a.Email,
										  Phone = a.PhoneNumber,
										  FileDataStore = a.FileDataUrl,
										  PositionId = p.Id,
										  PositionName = p.Name,
										  Rate = a.Rate,
										  TestId = t.Id,
										  TestName = t.Name,
										  InterviewerName = a.InterviewerName
									  }).ToListAsync(),
					IsSuccess = true
				};
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<ApiResponse<bool>> RemoveApplicant(int id)
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

		public Task<ApiResponse<bool>> UpdateApplicant(int id, ApplicantUpsert applicantUpdate)
		{
			throw new NotImplementedException();
		}
	}
}
