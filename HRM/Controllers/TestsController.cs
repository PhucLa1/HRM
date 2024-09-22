using Asp.Versioning;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using HRM.Services.RecruitmentManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Apis.Controllers
{
	[ApiVersion(1)]
	[Route("api/v{v:apiVersion}/tests")]
	[ApiController]
	[Authorize(Policy = RoleExtensions.HR_ROLE)]
	public class TestsController : ControllerBase
	{
		private readonly ITestsService _testService;
		public TestsController(ITestsService testService)
		{
			_testService = testService;
		}

		/// <summary>
		/// Get all tests in company
		/// </summary>
		/// <response code="200">Return all the tests in the company in the metadata of api response</response>
		[HttpGet]
		[Route("")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<TestResult>>))]
		//[SwaggerResponseExample(StatusCodes.Status200OK, typeof(TestResponseExample))]
		public async Task<IActionResult> GetAll()
		{
			return Ok(await _testService.GetAllTest());
		}


		/// <summary>
		/// Add new test in the company
		/// </summary>
		/// <response code="200">Return the api response</response>
		[HttpPost]
		[Route("")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
		public async Task<IActionResult> AddNew([FromBody] TestUpsert testAdd)
		{
			return Ok(await _testService.AddNewTest(testAdd));
		}


		/// <summary>
		/// Update a test in the company by id
		/// </summary>
		/// <response code="200">Return the api response </response>
		[HttpPut]
		[Route("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
		public async Task<IActionResult> UpdateTest(int id, [FromBody] TestUpsert testUpdate)
		{
			return Ok(await _testService.UpdateTest(id, testUpdate));
		}


		/// <summary>
		/// Delete a test in the company by id
		/// </summary>
		/// <response code="200">Return the api response </response>
		[HttpDelete]
		[Route("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
		public async Task<IActionResult> RemoveTest(int id)
		{
			return Ok(await _testService.RemoveTest(id));
		}
	}
}
