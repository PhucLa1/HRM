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
    [Route("api/v{v:apiVersion}/positions")]
    [ApiController]
    [Authorize(Policy = "AdminRole")]
    public class PositionsController : ControllerBase
    {
        private readonly IPositionsService _positionService;
        public PositionsController(IPositionsService positionService)
        {
            _positionService = positionService;
        }


        /// <summary>
        /// Get all positions in company
        /// </summary>
        /// <response code="200">Return all the positions in the company in the metadata of api response</response>
        [HttpGet] 
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<PositionResult>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(PositionResponseExample))]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _positionService.GetAllPosition());
        }


        /// <summary>
        /// Add new position in the company
        /// </summary>
        /// <response code="200">Return the api response</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> AddNew([FromBody] PositionUpsert positionAdd)
        {
            return Ok(await _positionService.AddNewPosition(positionAdd));
        }


        /// <summary>
        /// Update a position in the company by id
        /// </summary>
        /// <response code="200">Return the api response </response>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> UpdatePosition(int id, [FromBody] PositionUpsert positionUpdate)
        {
            return Ok(await _positionService.UpdatePosition(id, positionUpdate));
        }


        /// <summary>
        /// Delete a position in the company by id
        /// </summary>
        /// <response code="200">Return the api response </response>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> RemovePosition(int id)
        {
            return Ok(await _positionService.RemovePosition(id));
        }
    }
}
