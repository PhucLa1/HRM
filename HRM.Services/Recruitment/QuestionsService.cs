using AutoMapper;
using FluentValidation;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using Microsoft.EntityFrameworkCore;

namespace HRM.Services.RecruitmentManager
{
    public interface IQuestionsService
	{
		Task<ApiResponse<IEnumerable<QuestionResult>>> GetAllQuestion();
		Task<ApiResponse<bool>> AddNewQuestion(QuestionUpsert questionAdd);
		Task<ApiResponse<bool>> UpdateQuestion(int id, QuestionUpsert questionUpdate);
		Task<ApiResponse<bool>> RemoveQuestion(int id);
	}
	public class QuestionsService : IQuestionsService
	{
		private readonly IBaseRepository<Questions> _baseRepository;
		private readonly IBaseRepository<Test> _baseTestRepository;
		private readonly IValidator<QuestionUpsert> _questionUpsertValidator;
		private readonly IMapper _mapper;
		public QuestionsService(
			IBaseRepository<Questions> baseRepository,
			IBaseRepository<Test> baseTestRepository,
			IValidator<QuestionUpsert> questionUpsertValidator,
			IMapper mapper)
		{
			_baseRepository = baseRepository;
			_baseTestRepository = baseTestRepository;
			_questionUpsertValidator = questionUpsertValidator;
			_mapper = mapper;
		}
		public async Task<ApiResponse<IEnumerable<QuestionResult>>> GetAllQuestion()
		{
			try
			{
				return new ApiResponse<IEnumerable<QuestionResult>>
				{
					Metadata = await (from q in _baseRepository.GetAllQueryAble()
									  join t in _baseTestRepository.GetAllQueryAble()
									  on q.TestId equals t.Id
									  select new QuestionResult
									  {
										  Id = q.Id,
										  TestId = q.TestId,
										  TestName = t.Name,
										  QuestionText = q.QuestionText,
										  Point = q.Point
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
		public async Task<ApiResponse<bool>> AddNewQuestion(QuestionUpsert questionAdd)
		{
			try
			{
				var testName = await _baseTestRepository.GetAllQueryAble().Where(e => e.Id == questionAdd.TestId).FirstAsync();
				var resultValidation = _questionUpsertValidator.Validate(questionAdd);
				if (!resultValidation.IsValid)
				{
					return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
				}
				await _baseRepository.AddAsync(new Questions { 
					TestId = questionAdd.TestId ,
					QuestionText = questionAdd.QuestionText.Trim(), 
					Point = questionAdd.Point 
				});
				await _baseRepository.SaveChangeAsync();
				return new ApiResponse<bool> { IsSuccess = true };
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public async Task<ApiResponse<bool>> UpdateQuestion(int id, QuestionUpsert questionUpdate)
		{
			try
			{
				var resultValidation = _questionUpsertValidator.Validate(questionUpdate);
				if (!resultValidation.IsValid)
				{
					return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
				}
				var postion = await _baseRepository.GetAllQueryAble().Where(e => e.Id == id).FirstAsync();
				postion.TestId = questionUpdate.TestId;
				postion.QuestionText = questionUpdate.QuestionText.Trim();
				postion.Point = questionUpdate.Point;
				_baseRepository.Update(postion);
				await _baseRepository.SaveChangeAsync();
				return new ApiResponse<bool> { IsSuccess = true };
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public async Task<ApiResponse<bool>> RemoveQuestion(int id)
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
