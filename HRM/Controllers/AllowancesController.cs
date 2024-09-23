using Asp.Versioning;
using HRM.Apis.Swagger.Examples.Responses;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using HRM.Services.Briefcase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Controllers
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/allowances")]
    [ApiController]
    [Authorize(Policy = "AdminRole")]
    public class AllowancesController : ControllerBase
    {
        private readonly IAllowancesService _allowanceService;
        public AllowancesController(IAllowancesService allowanceService)
        {
            _allowanceService = allowanceService;
        }


        /// <summary>
        /// Get all allowances in company
        /// </summary>
        /// <response code="200">Return all the allowances in the company in the metadata of api response</response>
        [HttpGet] 
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<AllowanceResult>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AllowanceResponseExample))]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _allowanceService.GetAllAllowance());
        }


        /// <summary>
        /// Add new allowance in the company
        /// </summary>
        /// <response code="200">Return the api response</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> AddNew([FromBody] AllowanceUpsert allowanceAdd)
        {
            return Ok(await _allowanceService.AddNewAllowance(allowanceAdd));
        }


        /// <summary>
        /// Update a allowance in the company by id
        /// </summary>
        /// <response code="200">Return the api response </response>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> UpdateAllowance(int id, [FromBody] AllowanceUpsert allowanceUpdate)
        {
            return Ok(await _allowanceService.UpdateAllowance(id, allowanceUpdate));
        }


        /// <summary>
        /// Delete a allowance in the company by id
        /// </summary>
        /// <response code="200">Return the api response </response>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> RemoveAllowance(int id)
        {
            return Ok(await _allowanceService.RemoveAllowance(id));
        }
    }
}
