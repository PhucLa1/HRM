using Asp.Versioning;
using HRM.Apis.Swagger.Examples.Responses;
using HRM.Data.Entities;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using HRM.Services.Recruitment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Controllers
{
	[ApiVersion(1)]
	[Route("api/v{v:apiVersion}/recruitmentwebs")]
	[ApiController]
	[Authorize(Policy = RoleExtensions.ADMIN_ROLE)]
	public class RecruitmentWebsController : ControllerBase
	{
		private readonly IRecruitmentWebsService _recruitmentWebService;
		public RecruitmentWebsController(IRecruitmentWebsService recruitmentWebService)
		{
			_recruitmentWebService = recruitmentWebService;
		}


		/// <summary>
		/// Get all recruitmentWebs in company
		/// </summary>
		/// <response code="200">Return all the recruitmentWebs in the company in the metadata of api response</response>
		[HttpGet]
		[Route("")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<RecruitmentWebResult>>))]
		[SwaggerResponseExample(StatusCodes.Status200OK, typeof(RecruitmentWebResponseExample))]
		public async Task<IActionResult> GetAll()
		{
			return Ok(await _recruitmentWebService.GetAllRecruitmentWeb());
		}


		/// <summary>
		/// Add new recruitmentWeb in the company
		/// </summary>
		/// <response code="200">Return the api response</response>
		[HttpPost]
		[Route("")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
		public async Task<IActionResult> AddNew([FromBody] RecruitmentWebUpsert recruitmentWebAdd)
		{
			return Ok(await _recruitmentWebService.AddNewRecruitmentWeb(recruitmentWebAdd));
		}


		/// <summary>
		/// Update a recruitmentWeb in the company by id
		/// </summary>
		/// <response code="200">Return the api response </response>
		/*[HttpPut]
		[Route("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
		public async Task<IActionResult> UpdateRecruitmentWeb(int id, [FromBody] RecruitmentWebUpsert recruitmentWebUpdate)
		{
			return Ok(await _recruitmentWebService.UpdateRecruitmentWeb(id, recruitmentWebUpdate));
		}*/


		/// <summary>
		/// Delete a recruitmentWeb in the company by id
		/// </summary>
		/// <response code="200">Return the api response </response>
		[HttpDelete]
		[Route("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
		public async Task<IActionResult> RemoveRecruitmentWeb(int id)
		{
			return Ok(await _recruitmentWebService.RemoveRecruitmentWeb(id));
		}
	}
}
