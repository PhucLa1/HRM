using AutoMapper;
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
	public interface ITestResultsService
	{
		Task<ApiResponse<IEnumerable<TestResultResult>>> GetAllTestResult();
		Task<ApiResponse<IEnumerable<TestResultResult>>> GetAllTestResultByTestId(int id);
		Task<ApiResponse<bool>> AddNewTestResult(TestResultUpsert testResultAdd);
		Task<ApiResponse<bool>> AddNewTestResultList(List<TestResultUpsert> testResults);
		Task<ApiResponse<bool>> UpdateTestResult(int id, TestResultUpsert testResultUpdate);
		Task<ApiResponse<bool>> RemoveTestResult(int id);
	}
	public class TestResultsService : ITestResultsService
	{
		private readonly IBaseRepository<TestResult> _baseRepository;
		private readonly IBaseRepository<Questions> _questionRepository;
		private readonly IBaseRepository<Applicants> _applicantRepository;
		private readonly IBaseRepository<Test> _testRepository;
		private readonly IValidator<TestResultUpsert> _testResultUpsertValidator;
		private readonly IMapper _mapper;
		public TestResultsService(
			IBaseRepository<TestResult> baseRepository,
			IBaseRepository<Questions> questionRepository,
			IBaseRepository<Applicants> applicantRepository,
			IBaseRepository<Test> testRepository,
		IValidator<TestResultUpsert> testResultUpsertValidator,
			IMapper mapper)
		{
			_baseRepository = baseRepository;
			_questionRepository = questionRepository;
			_applicantRepository = applicantRepository;
			_testRepository = testRepository;
			_testResultUpsertValidator = testResultUpsertValidator;
			_mapper = mapper;
		}
		public async Task<ApiResponse<bool>> AddNewTestResult(TestResultUpsert testResultAdd)
		{
			try
			{
				var resultValidation = _testResultUpsertValidator.Validate(testResultAdd);
				if (!resultValidation.IsValid)
				{
					return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
				}
				await _baseRepository.AddAsync(
					new TestResult { 
						ApplicantId = testResultAdd.ApplicantId,
						QuestionsId = testResultAdd.QuestionsId,
						Point = testResultAdd.Point,
						Comment = testResultAdd.Comment
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

		public async Task<ApiResponse<bool>> AddNewTestResultList(List<TestResultUpsert> testResults)
		{
			try
			{
				foreach (var testResultAdd in testResults)
				{
					// Kiểm tra hợp lệ cho từng đối tượng
					var resultValidation = _testResultUpsertValidator.Validate(testResultAdd);
					if (!resultValidation.IsValid)
					{
						return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
					}

					// Kiểm tra xem TestResult đã tồn tại chưa
					var existingResults = await (from tr in _baseRepository.GetAllQueryAble()
												 where tr.ApplicantId == testResultAdd.ApplicantId && tr.QuestionsId == testResultAdd.QuestionsId
												 select tr).FirstOrDefaultAsync();

					if (existingResults != null)
					{
						// Nếu tồn tại, cập nhật điểm và nhận xét
						existingResults.Point = testResultAdd.Point;
						existingResults.Comment = testResultAdd.Comment;
						_baseRepository.Update(existingResults); // Cập nhật đối tượng
					}
					else
					{
						// Nếu không tồn tại, thêm mới
						await _baseRepository.AddAsync(
							new TestResult
							{
								ApplicantId = testResultAdd.ApplicantId,
								QuestionsId = testResultAdd.QuestionsId,
								Point = testResultAdd.Point,
								Comment = testResultAdd.Comment
							}
						);
					}
				}

				await _baseRepository.SaveChangeAsync();
				return new ApiResponse<bool> { IsSuccess = true };
			}
			catch (Exception ex)
			{
				// Xử lý ngoại lệ và trả về thông báo lỗi
				throw new Exception(ex.Message);
			}
		}




		public async Task<ApiResponse<IEnumerable<TestResultResult>>> GetAllTestResult()
		{
			try
			{
				return new ApiResponse<IEnumerable<TestResultResult>>
				{
					Metadata = await (from tr in _baseRepository.GetAllQueryAble()
									  join q in _questionRepository.GetAllQueryAble() on tr.QuestionsId equals q.Id into questionJoin
									  from q in questionJoin.DefaultIfEmpty()
									  join a in _applicantRepository.GetAllQueryAble() on tr.ApplicantId equals a.Id into applicantJoin
									  from a in applicantJoin.DefaultIfEmpty()
									  join t in _testRepository.GetAllQueryAble() on a.TestId equals t.Id into testJoin
									  from t in testJoin.DefaultIfEmpty()
									  select new TestResultResult
									  {
										  Id = tr.Id,
										  ApplicantId = tr.ApplicantId,
										  QuestionsId = tr.QuestionsId,
										  ApplicantTestId = a.TestId,
										  Point = tr.Point,
										  Comment = tr.Comment,
									  }).ToListAsync(),
					IsSuccess = true
				};
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<ApiResponse<IEnumerable<TestResultResult>>> GetAllTestResultByTestId(int testId)
		{
			try
			{
				return new ApiResponse<IEnumerable<TestResultResult>>
				{
					Metadata = await(from tr in _baseRepository.GetAllQueryAble()
									 join q in _questionRepository.GetAllQueryAble() on tr.QuestionsId equals q.Id into questionJoin
									 from q in questionJoin.DefaultIfEmpty()
									 join a in _applicantRepository.GetAllQueryAble() on tr.ApplicantId equals a.Id into applicantJoin
									 from a in applicantJoin.DefaultIfEmpty()
									 join t in _testRepository.GetAllQueryAble() on a.TestId equals t.Id into testJoin
									 from t in testJoin.DefaultIfEmpty()
									 where q.TestId == testId && a.TestId == testId
									 select new TestResultResult
									 {
										 Id = tr.Id,
										 ApplicantId = tr.ApplicantId,
										 QuestionsId = tr.QuestionsId,
										 ApplicantTestId = a.TestId,
										 Point = tr.Point,
										 Comment = tr.Comment
									 }).ToListAsync(),
					IsSuccess = true
				};
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		public async Task<ApiResponse<bool>> RemoveTestResult(int id)
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

		public async Task<ApiResponse<bool>> UpdateTestResult(int id, TestResultUpsert testResultUpdate)
		{
			try
			{
				var resultValidation = _testResultUpsertValidator.Validate(testResultUpdate);
				if (!resultValidation.IsValid)
				{
					return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
				}
				var testResult = await _baseRepository.GetAllQueryAble().Where(e => e.Id == id).FirstAsync();

				testResult.ApplicantId = testResultUpdate.ApplicantId;
				testResult.QuestionsId = testResultUpdate.QuestionsId;
				testResult.Point = testResultUpdate.Point;
				testResult.Comment = testResultUpdate.Comment;

				_baseRepository.Update(testResult);
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
