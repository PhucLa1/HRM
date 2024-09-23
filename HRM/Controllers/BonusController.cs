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
    [Route("api/v{v:apiVersion}/bonus")]
    [ApiController]
    //[Authorize(Policy = "AdminRole")]
    public class BonusController : ControllerBase
    {
        private readonly IBonusService _bonusService;
        public BonusController(IBonusService bonusService)
        {
            _bonusService = bonusService;
        }


        /// <summary>
        /// Get all bonus defination
        /// </summary>
        /// <response code="200">Return all the bonus defination in the metadata of api response</response>
        [HttpGet] 
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<BonusResult>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(BonusResponseExample))]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _bonusService.GetAllBonus());
        }


        /// <summary>
        /// Add new bonus defination
        /// </summary>
        /// <response code="200">Return the api response when inserted succesfully</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [SwaggerRequestExample(typeof(BonusUpsert), typeof(BonusUpsertRequestExample))]
        public async Task<IActionResult> AddNew([FromBody] BonusUpsert bonusAdd)
        {
            return Ok(await _bonusService.AddNewBonus(bonusAdd));
        }


        /// <summary>
        /// Update a bonus defination by id
        /// </summary>
        /// <response code="200">Return the api response when updated succesfully</response>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [SwaggerRequestExample(typeof(BonusUpsert), typeof(BonusUpsertRequestExample))]
        public async Task<IActionResult> UpdateBonus(int id, [FromBody] BonusUpsert bonusUpdate)
        {
            return Ok(await _bonusService.UpdateBonus(id, bonusUpdate));
        }


        /// <summary>
        /// Delete a bonus defination by id
        /// </summary>
        /// <response code="200">Return the api response when deleted succesfully</response>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> RemoveBonus(int id)
        {
            return Ok(await _bonusService.RemoveBonus(id));
        }
    }
}
