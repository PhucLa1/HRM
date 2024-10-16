using Asp.Versioning;
using HRM.Apis.Swagger.Examples.Requests;
using HRM.Apis.Swagger.Examples.Responses;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using HRM.Services.Salary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Controllers
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/advances")]
    [ApiController]
    //[Authorize(Policy = "AdminRole")]
    public class AdvanceController : ControllerBase
    {
        private readonly IAdvancesService _advancesService;
        public AdvanceController(IAdvancesService advancesService)
        {
            _advancesService = advancesService;
        }


        /// <summary>
        /// Get all advance defination
        /// </summary>
        /// <response code="200">Return all the advance defination in the metadata of api response</response>
        [HttpGet] 
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<AdvanceResult>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ApiResponse<AdvanceResult>))]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _advancesService.GetAllAdvance());
        }


        /// <summary>
        /// Add new advance defination
        /// </summary>
        /// <response code="200">Return the api response when inserted succesfully</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [SwaggerRequestExample(typeof(AdvanceUpsert), typeof(AdvanceUpsert))]
        public async Task<IActionResult> AddNew([FromBody] AdvanceUpsert advanceAdd)
        {
            return Ok(await _advancesService.AddNewAdvance(advanceAdd));
        }


        /// <summary>
        /// Update a advance defination by id
        /// </summary>
        /// <response code="200">Return the api response when updated succesfully</response>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [SwaggerRequestExample(typeof(AdvanceUpsert), typeof(AdvanceUpsert))]
        public async Task<IActionResult> UpdateAdvance(int id, [FromBody] AdvanceUpsert advanceUpdate)
        {
            return Ok(await _advancesService.UpdateAdvance(id, advanceUpdate));
        }


        /// <summary>
        /// Delete a advance defination by id
        /// </summary>
        /// <response code="200">Return the api response when deleted succesfully</response>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> RemoveAdvance(int id)
        {
            return Ok(await _advancesService.RemoveAdvance(id));
        }
    }
}
