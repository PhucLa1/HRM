using Asp.Versioning;
using HRM.Apis.Swagger.Examples.Responses;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using HRM.Services.RecruitmentManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Controllers
{
    [ApiVersion(1)]
	[Route("api/v{v:apiVersion}/webs")]
	[ApiController]
	[Authorize(Policy = RoleExtensions.ADMIN_ROLE)]
	public class WebsController : ControllerBase
	{
		private readonly IWebsService _webService;
		public WebsController(IWebsService webService)
		{
			_webService = webService;
		}

		/// <summary>
		/// Get all webs
		/// </summary>
		/// <response code="200">Return all the webs in the company in the metadata of api response</response>
		[HttpGet]
		[Route("")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<WebResult>>))]
		[SwaggerResponseExample(StatusCodes.Status200OK, typeof(WebResponseExample))]
		public async Task<IActionResult> GetAll()
		{
			return Ok(await _webService.GetAllWeb());
		}


		/// <summary>
		/// Add new webs
		/// </summary>
		/// <response code="200">Return the api response</response>
		[HttpPost]
		[Route("")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
		public async Task<IActionResult> AddNew([FromBody] WebUpsert webAdd)
		{
			return Ok(await _webService.AddNewWeb(webAdd));
		}


		/// <summary>
		/// Update a web in the company by id
		/// </summary>
		/// <response code="200">Return the api response </response>
		[HttpPut]
		[Route("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
		public async Task<IActionResult> UpdateWeb(int id, [FromBody] WebUpsert webUpdate)
		{
			return Ok(await _webService.UpdateWeb(id, webUpdate));
		}


		/// <summary>
		/// Delete a web by id
		/// </summary>
		/// <response code="200">Return the api response </response>
		[HttpDelete]
		[Route("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
		public async Task<IActionResult> RemoveWeb(int id)
		{
			return Ok(await _webService.RemoveWeb(id));
		}
	}
}
