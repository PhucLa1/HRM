using Asp.Versioning;
using HRM.Apis.Swagger.Examples.Responses;
using HRM.Data.Entities;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using HRM.Services.Recruitment;
using HRM.Services.RecruitmentManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Controllers
{
	[ApiVersion(1)]
	[Route("api/v{v:apiVersion}/testResults")]
	[ApiController]
	[Authorize(Policy = RoleExtensions.ADMIN_ROLE)]
	public class TestResultsController : ControllerBase
	{
		private readonly ITestResultsService _testResultService;
		public TestResultsController(ITestResultsService testResultService)
		{
			_testResultService = testResultService;
		}

		/// <summary>
		/// Get all testResults
		/// </summary>
		/// <response code="200">Return all the testResults in the company in the metadata of api response</response>
		[HttpGet]
		[Route("")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<TestResultResult>>))]
		[SwaggerResponseExample(StatusCodes.Status200OK, typeof(TestResultResponseExample))]
		public async Task<IActionResult> GetAll()
		{
			return Ok(await _testResultService.GetAllTestResult());
		}

		/// <summary>
		/// Get all testResults by testId
		/// </summary>
		/// <response code="200">Return all the testResults in the company by testId in the metadata of api response</response>
		[HttpGet]
		[Route("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<TestResultResult>>))]
		[SwaggerResponseExample(StatusCodes.Status200OK, typeof(TestResultResponseExample))]
		public async Task<IActionResult> GetAllTestResultByTestId(int id)
		{
			return Ok(await _testResultService.GetAllTestResultByTestId(id));
		}


		/// <summary>
		/// Add new testResults
		/// </summary>
		/// <response code="200">Return the api response</response>
		[HttpPost]
		[Route("")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
		public async Task<IActionResult> AddNew([FromBody] TestResultUpsert testResultAdd)
		{
			return Ok(await _testResultService.AddNewTestResult(testResultAdd));
		}

		/// <summary>
		/// Add new testResults
		/// </summary>
		/// <response code="200">Return the api response</response>
		[HttpPost]
		[Route("list")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
		public async Task<IActionResult> AddNewList([FromBody] List<TestResultUpsert> testResultAdd)
		{
			return Ok(await _testResultService.AddNewTestResultList(testResultAdd));
		}


		/// <summary>
		/// Update a testResult in the company by id
		/// </summary>
		/// <response code="200">Return the api response </response>
		[HttpPut]
		[Route("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
		public async Task<IActionResult> UpdateTestResult(int id, [FromBody] TestResultUpsert testResultUpdate)
		{
			return Ok(await _testResultService.UpdateTestResult(id, testResultUpdate));
		}


		/// <summary>
		/// Delete a testResult by id
		/// </summary>
		/// <response code="200">Return the api response </response>
		[HttpDelete]
		[Route("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
		public async Task<IActionResult> RemoveTestResult(int id)
		{
			return Ok(await _testResultService.RemoveTestResult(id));
		}
	}
}
