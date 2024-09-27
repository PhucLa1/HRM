using Asp.Versioning;
using HRM.Apis.Swagger.Examples.Responses;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using HRM.Services.RecruitmentManager;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Controllers
{
    [ApiVersion(1)]
	[Route("api/v{v:apiVersion}/jobs")]
	[ApiController]
	//[Authorize(Policy = RoleExtensions.HR_ROLE)]
	public class JobsController : ControllerBase
	{
		private readonly IJobsService _jobService;
		public JobsController(IJobsService jobService)
		{
			_jobService = jobService;
		}


		/// <summary>
		/// Get all jobs in company
		/// </summary>
		/// <response code="200">Return all the jobs in the company in the metadata of api response</response>
		[HttpGet]
		[Route("")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<JobResult>>))]
		[SwaggerResponseExample(StatusCodes.Status200OK, typeof(JobResponseExample))]
		public async Task<IActionResult> GetAll()
		{
			return Ok(await _jobService.GetAllJob());
		}


		/// <summary>
		/// Add new job in the company
		/// </summary>
		/// <response code="200">Return the api response</response>
		[HttpPost]
		[Route("")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
		public async Task<IActionResult> AddNew([FromBody] JobUpsert jobAdd)
		{
			return Ok(await _jobService.AddNewJob(jobAdd));
		}


		/// <summary>
		/// Update a job in the company by id
		/// </summary>
		/// <response code="200">Return the api response </response>
		[HttpPut]
		[Route("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
		public async Task<IActionResult> UpdateJob(int id, [FromBody] JobUpsert jobUpdate)
		{
			return Ok(await _jobService.UpdateJob(id, jobUpdate));
		}


		/// <summary>
		/// Delete a job in the company by id
		/// </summary>
		/// <response code="200">Return the api response </response>
		[HttpDelete]
		[Route("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
		public async Task<IActionResult> RemoveJob(int id)
		{
			return Ok(await _jobService.RemoveJob(id));
		}
	}
}
