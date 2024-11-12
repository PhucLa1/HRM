using Asp.Versioning;
using HRM.Apis.Swagger.Examples.Responses;
using HRM.Data.Entities;
using HRM.Repositories.Dtos.Results;
using HRM.Services.Recruitment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Controllers
{
    [ApiVersion(1)]
	[Route("api/v{v:apiVersion}/gmails")]
	[ApiController]
	[Authorize(Policy = RoleExtensions.ADMIN_ROLE)]
	public class GmailsController : ControllerBase
	{
		private readonly IGmailsService _gmailService;
		public GmailsController(IGmailsService gmailService)
		{
			_gmailService = gmailService;
		}

		/// <summary>
		/// Get all gmails
		/// </summary>
		/// <response code="200">Return all the gmails in the company in the metadata of api response</response>
		[HttpGet]
		[Route("")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<WebResult>>))]
		[SwaggerResponseExample(StatusCodes.Status200OK, typeof(WebResponseExample))]
		public async Task<IActionResult> GetAll()
		{
			return Ok(await _gmailService.GetAllGmail());
		}
	}
}
