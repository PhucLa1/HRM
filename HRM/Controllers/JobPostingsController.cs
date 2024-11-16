using Asp.Versioning;
using HRM.Apis.Swagger.Examples.Responses;
using HRM.Data.Entities;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using HRM.Services.Recruitment;
using HRM.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Controllers
{
    [ApiVersion(1)]
	[Route("api/v{v:apiVersion}/jobpostings")]
	[ApiController]
	[Authorize(Policy = RoleExtensions.ADMIN_ROLE)]
	public class JobPostingsController : ControllerBase
	{
		private readonly IJobPostingsService _jobPostingService;
		public JobPostingsController(IJobPostingsService jobPostingService)
		{
			_jobPostingService = jobPostingService;
		}


		/// <summary>
		/// Get all jobPostings in company
		/// </summary>
		/// <response code="200">Return all the jobPostings in the company in the metadata of api response</response>
		[HttpGet]
		[Route("")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<JobPostingResult>>))]
		[SwaggerResponseExample(StatusCodes.Status200OK, typeof(JobPostingResponseExample))]
		public async Task<IActionResult> GetAll()
		{
			return Ok(await _jobPostingService.GetAllJobPosting());
		}


		/// <summary>
		/// Add new jobPosting in the company
		/// </summary>
		/// <response code="200">Return the api response</response>
		[HttpPost]
		[Route("")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
		public async Task<IActionResult> AddNew([FromBody] JobPostingUpsert jobPostingAdd)
		{
			return Ok(await _jobPostingService.AddNewJobPosting(jobPostingAdd));
		}


		/// <summary>
		/// Update a jobPosting in the company by id
		/// </summary>
		/// <response code="200">Return the api response </response>
		[HttpPut]
		[Route("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
		public async Task<IActionResult> UpdateJobPosting(int id, [FromBody] JobPostingUpsert jobPostingUpdate)
		{
			return Ok(await _jobPostingService.UpdateJobPosting(id, jobPostingUpdate));
		}


		/// <summary>
		/// Delete a jobPosting in the company by id
		/// </summary>
		/// <response code="200">Return the api response </response>
		[HttpDelete]
		[Route("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
		public async Task<IActionResult> RemoveJobPosting(int id)
		{
			return Ok(await _jobPostingService.RemoveJobPosting(id));
		}
	}
}
