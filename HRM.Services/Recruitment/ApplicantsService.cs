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
		Task<ApiResponse<IEnumerable<ApplicantResult>>> GetApplicantById(int id);
		Task<ApiResponse<bool>> AddNewApplicant(ApplicantUpsert applicantAdd);
		Task<ApiResponse<bool>> UpdateApplicant(int id, ApplicantUpsert applicantUpdate);
		Task<ApiResponse<bool>> UpdateApplicantPoint(int id, double point);
		Task<ApiResponse<bool>> RemoveApplicant(int id);
	}
	public class ApplicantsService : IApplicantsService
	{
		private readonly IBaseRepository<Applicants> _baseRepository;
		private readonly IBaseRepository<Test> _testRepository;
		private readonly IBaseRepository<Position> _positionRepository;
		private readonly IBaseRepository<TestResult> _testResultRepository;
		private readonly IValidator<ApplicantUpsert> _applicantUpsertValidator;
		private readonly IMapper _mapper;
		public ApplicantsService(
			IBaseRepository<Applicants> baseRepository,
			IValidator<ApplicantUpsert> applicantUpsertValidator,
			IBaseRepository<Test> testRepository,
			IBaseRepository<TestResult> testResultRepository,
		IBaseRepository<Position> positionRepository,
			IMapper mapper)
		{
			_baseRepository = baseRepository;
			_applicantUpsertValidator = applicantUpsertValidator;
			_positionRepository = positionRepository;
			_testRepository = testRepository;
			_testResultRepository = testResultRepository;
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
					Rate = applicantAdd.Rate ?? null,
					TestId = applicantAdd.TestId ?? null,
					InterviewerName = applicantAdd.InterviewerName
				};
				if (applicantAdd.file.Length > 0)
				{
					string folder = "CV"; // Target folder for CV uploads in wwwroot
					applicant.FileDataUrl = HandleFile.UPLOAD_GETPATH(folder, applicantAdd.file);
				}

				await _baseRepository.AddAsync(applicant);
				await _baseRepository.SaveChangeAsync();
				return new ApiResponse<bool> { IsSuccess = true };
			}
			catch (Exception ex)
			{
				var innerException = ex.InnerException?.Message ?? ex.Message;
				throw new Exception($"An error occurred: {innerException}");
			}
		}

		public async Task<ApiResponse<IEnumerable<ApplicantResult>>> GetAllApplicant()
		{
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

		public async Task<ApiResponse<IEnumerable<ApplicantResult>>> GetApplicantById(int id)
		{
			try
			{
				return new ApiResponse<IEnumerable<ApplicantResult>>
				{
					Metadata = await (from a in _baseRepository.GetAllQueryAble()
									  join p in _positionRepository.GetAllQueryAble() on a.PositionId equals p.Id into positionJoin
									  from p in positionJoin.DefaultIfEmpty()
									  join t in _testRepository.GetAllQueryAble() on a.TestId equals t.Id into testJoin
									  from t in testJoin.DefaultIfEmpty()
									  where a.Id == id
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

		public async Task<ApiResponse<bool>> UpdateApplicant(int id, ApplicantUpsert applicantUpdate)
		{
			try
			{
				var resultValidation = _applicantUpsertValidator.Validate(applicantUpdate);
				if (!resultValidation.IsValid)
				{
					return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
				}
				var applicant = await _baseRepository.GetAllQueryAble().Where(e => e.Id == id).FirstAsync();
				//Nhập lại dữ liệu
				applicant.Name = applicantUpdate.Name!.Trim();
				applicant.Email = applicantUpdate.Email!;
				applicant.PhoneNumber = applicantUpdate.Phone;
				applicant.PositionId = applicantUpdate.PositionId;
				applicant.Rate = applicantUpdate.Rate ?? null;
				applicant.TestId = applicantUpdate.TestId ?? null;
				applicant.InterviewerName = applicantUpdate.InterviewerName;
				_baseRepository.Update(applicant);
				await _baseRepository.SaveChangeAsync();
				return new ApiResponse<bool> { IsSuccess = true };
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<ApiResponse<bool>> UpdateApplicantPoint(int id, double point)
		{
			try
			{
				// Lấy thông tin ứng viên theo ID
				var applicant = await _baseRepository.GetAllQueryAble().Where(e => e.Id == id).FirstOrDefaultAsync();
				if (applicant == null)
				{
					return ApiResponse<bool>.FailtureValidation(applicant);
				}
				// Lấy toàn bộ điểm từ TestResult của ứng viên
				var testResults = await _testResultRepository.GetAllQueryAble()
					.Where(e => e.ApplicantId == id)
					.ToListAsync();

				// Tính tổng điểm và số lượng câu hỏi
				double totalPoints = testResults.Sum(tr => tr.Point);
				int questionCount = testResults.Count;

				// Tính tỷ lệ
				//applicant.Rate = questionCount > 0 ? totalPoints / questionCount : 0;
				applicant.Rate = totalPoints; // Tránh chia cho 0

				// Cập nhật thông tin ứng viên
				_baseRepository.Update(applicant);
				await _baseRepository.SaveChangeAsync();

				return new ApiResponse<bool> { IsSuccess = true };
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message); // Trả về thông báo lỗi nếu có ngoại lệ
			}
		}


		//public async Task<ApiResponse<bool>> UpdateApplicantPoint(int id, double point)
		//{
		//	try
		//	{
		//		var applicant = await _baseRepository.GetAllQueryAble().Where(e => e.Id == id).FirstAsync();
		//		var testResult = await _testResultRepository.GetAllQueryAble().Where(e => e.ApplicantId == id).ToListAsync();
		//		//Nhập lại dữ liệu
		//		applicant.Rate = point;
		//		_baseRepository.Update(applicant);
		//		await _baseRepository.SaveChangeAsync();
		//		return new ApiResponse<bool> { IsSuccess = true };
		//	}
		//	catch (Exception ex)
		//	{
		//		throw new Exception(ex.Message);
		//	}
		//}

		//public async Task<ApiResponse<bool>> UpdateApplicantTest(int id, ApplicantTestAdd applicantUpdate)
		//{
		//	try
		//	{
		//		var resultValidation = _applicantUpsertValidator.Validate();
		//		if (!resultValidation.IsValid)
		//		{
		//			return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
		//		}
		//		var applicant = await _baseRepository.GetAllQueryAble().Where(e => e.Id == id).FirstAsync();
		//		//Nhập lại dữ liệu
		//		applicant.TestId = applicantUpdate.TestId ?? null;
		//		//Lưu thông tin
		//		_baseRepository.Update(applicant);
		//		await _baseRepository.SaveChangeAsync();
		//		return new ApiResponse<bool> { IsSuccess = true };
		//	}
		//	catch (Exception ex)
		//	{
		//		throw new Exception(ex.Message);
		//	}
		//}
	}
}
