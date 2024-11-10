using Asp.Versioning;
using HRM.Apis.Swagger.Examples.Responses;
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
	[Route("api/v{v:apiVersion}/applicants")]
	[ApiController]
	[Authorize(Policy = RoleExtensions.ADMIN_ROLE)]
	public class ApplicantsController : ControllerBase
    {
		private readonly IApplicantsService _applicantService;
		public ApplicantsController(IApplicantsService applicantService)
		{
			_applicantService = applicantService;
		}

		/// <summary>
		/// Get all applicants
		/// </summary>
		/// <response code="200">Return all the applicants in the company in the metadata of api response</response>
		[HttpGet]
		[Route("")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<ApplicantResult>>))]
		[SwaggerResponseExample(StatusCodes.Status200OK, typeof(ApplicantResponseExample))]
		public async Task<IActionResult> GetAll()
		{
			return Ok(await _applicantService.GetAllApplicant());
		}

		/// <summary>
		/// Get all applicants
		/// </summary>
		/// <response code="200">Return all the applicants in the company in the metadata of api response</response>
		[HttpGet]
		[Route("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<ApplicantResult>>))]
		[SwaggerResponseExample(StatusCodes.Status200OK, typeof(ApplicantResponseExample))]
		public async Task<IActionResult> GetById(int id)
		{
			return Ok(await _applicantService.GetApplicantById(id));
		}


		/// <summary>
		/// Add new applicants
		/// </summary>
		/// <response code="200">Return the api response</response>
		[HttpPost]
		[Route("")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
		public async Task<IActionResult> AddNew([FromForm] ApplicantUpsert applicantAdd)
		{
			return Ok(await _applicantService.AddNewApplicant(applicantAdd));
		}


		/// <summary>
		/// Update a applicant in the company by id
		/// </summary>
		/// <response code="200">Return the api response </response>
		[HttpPut]
		[Route("update/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
		public async Task<IActionResult> UpdateApplicant(int id, [FromBody] ApplicantUpsert applicantUpdate)
		{
			return Ok(await _applicantService.UpdateApplicant(id, applicantUpdate));
		}

		/// <summary>
		/// Update a applicant in the company by id
		/// </summary>
		/// <response code="200">Return the api response </response>
		[HttpPut]
		[Route("update-point/{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
		public async Task<IActionResult> UpdateApplicantPoint(int id, double point)
		{
			return Ok(await _applicantService.UpdateApplicantPoint(id, point));
		}


		/// <summary>
		/// Update a applicantTest in the company by id
		/// </summary>
		/// <response code="200">Return the api response </response>
		//[HttpPut]
		//[Route("{id}")]
		//[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
		//public async Task<IActionResult> UpdateApplicantTest(int id, [FromForm] ApplicantTestAdd applicantUpdate)
		//{
		//	return Ok(await _applicantService.UpdateApplicantTest(id, applicantUpdate));
		//}


		/// <summary>
		/// Delete a applicant by id
		/// </summary>
		/// <response code="200">Return the api response </response>
		[HttpDelete]
		[Route("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
		public async Task<IActionResult> RemoveApplicant(int id)
		{
			return Ok(await _applicantService.RemoveApplicant(id));
		}

	}
}
