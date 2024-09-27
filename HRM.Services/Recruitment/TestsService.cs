using AutoMapper;
using FluentValidation;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using Microsoft.EntityFrameworkCore;
using TestResults = HRM.Repositories.Dtos.Results.TestResults;


namespace HRM.Services.RecruitmentManager
{
    public interface ITestsService
	{
		Task<ApiResponse<IEnumerable<TestResults>>> GetAllTest();
		Task<ApiResponse<bool>> AddNewTest(TestUpsert testAdd);
		Task<ApiResponse<bool>> UpdateTest(int id, TestUpsert testUpdate);
		Task<ApiResponse<bool>> RemoveTest(int id);
	}
	public class TestsService : ITestsService
	{
		private readonly IBaseRepository<Test> _baseRepository;
		private readonly IValidator<TestUpsert> _testUpsertValidator;
		private readonly IMapper _mapper;
		public TestsService(
			IBaseRepository<Test> baseRepository,
			IValidator<TestUpsert> testUpsertValidator,
			IMapper mapper)
		{
			_baseRepository = baseRepository;
			_testUpsertValidator = testUpsertValidator;
			_mapper = mapper;
		}
		public async Task<ApiResponse<IEnumerable<TestResults>>> GetAllTest()
		{
			try
			{
				return new ApiResponse<IEnumerable<TestResults>>
				{
					Metadata = _mapper.Map<IEnumerable<TestResults>>(await _baseRepository.GetAllQueryAble().ToListAsync()),
					IsSuccess = true
				};
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public async Task<ApiResponse<bool>> AddNewTest(TestUpsert testAdd)
		{
			try
			{
				var resultValidation = _testUpsertValidator.Validate(testAdd);
				if (!resultValidation.IsValid)
				{
					return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
				}
				await _baseRepository.AddAsync(new Test { Name = testAdd.Name.Trim(), Description = testAdd.Description.Trim() });
				await _baseRepository.SaveChangeAsync();
				return new ApiResponse<bool> { IsSuccess = true };
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public async Task<ApiResponse<bool>> UpdateTest(int id, TestUpsert testUpdate)
		{
			try
			{
				var resultValidation = _testUpsertValidator.Validate(testUpdate);
				if (!resultValidation.IsValid)
				{
					return ApiResponse<bool>.FailtureValidation(resultValidation.Errors);
				}
				var postion = await _baseRepository.GetAllQueryAble().Where(e => e.Id == id).FirstAsync();
				postion.Name = testUpdate.Name.Trim();
				postion.Description = testUpdate.Description.Trim();
				_baseRepository.Update(postion);
				await _baseRepository.SaveChangeAsync();
				return new ApiResponse<bool> { IsSuccess = true };
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		public async Task<ApiResponse<bool>> RemoveTest(int id)
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
