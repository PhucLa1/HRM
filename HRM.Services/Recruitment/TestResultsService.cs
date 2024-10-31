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
		Task<ApiResponse<bool>> AddNewTestResult(TestResultUpsert testResultAdd);
		Task<ApiResponse<bool>> UpdateTestResult(int id, TestResultUpsert testResultUpdate);
		Task<ApiResponse<bool>> RemoveTestResult(int id);
	}
	public class TestResultsService : ITestResultsService
	{
		private readonly IBaseRepository<TestResult> _baseRepository;
		private readonly IBaseRepository<Questions> _questionRepository;
		private readonly IBaseRepository<Applicants> _applicantRepository;
		private readonly IValidator<TestResultUpsert> _testResultUpsertValidator;
		private readonly IMapper _mapper;
		public TestResultsService(
			IBaseRepository<TestResult> baseRepository,
			IBaseRepository<Questions> questionRepository,
			IBaseRepository<Applicants> applicantRepository,
			IValidator<TestResultUpsert> testResultUpsertValidator,
			IMapper mapper)
		{
			_baseRepository = baseRepository;
			_questionRepository = questionRepository;
			_applicantRepository = applicantRepository;
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
						Point = testResultAdd.Point
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
									  select new TestResultResult
									  {
										  Id = tr.Id,
										  ApplicantId = tr.ApplicantId,
										  ApplicantName = a.Name,
										  QuestionsId = tr.QuestionsId,
										  QuestionText = q.QuestionText,
										  Point = tr.Point

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

		public Task<ApiResponse<bool>> UpdateTestResult(int id, TestResultUpsert testResultUpdate)
		{
			throw new NotImplementedException();
		}
	}
}
