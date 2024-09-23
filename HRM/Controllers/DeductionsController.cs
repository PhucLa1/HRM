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
    [Route("api/v{v:apiVersion}/deductions")]
    [ApiController]
    //[Authorize(Policy = "AdminRole")]
    public class DeductionsController : ControllerBase
    {
        private readonly IDeductionsService _deductionService;
        public DeductionsController(IDeductionsService deductionService)
        {
            _deductionService = deductionService;
        }


        /// <summary>
        /// Get all deductions defination
        /// </summary>
        /// <response code="200">Return all the deductions defination in the metadata of api response</response>
        [HttpGet] 
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<DeductionResult>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(DeductionResponseExample))]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _deductionService.GetAllDeduction());
        }


        /// <summary>
        /// Add new deduction defination
        /// </summary>
        /// <response code="200">Return the api response when inserted successfully</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [SwaggerRequestExample(typeof(DeductionUpsert), typeof(DeductionUpsertRequestExample))]
        public async Task<IActionResult> AddNew([FromBody] DeductionUpsert deductionAdd)
        {
            return Ok(await _deductionService.AddNewDeduction(deductionAdd));
        }


        /// <summary>
        /// Update a deduction defination by id
        /// </summary>
        /// <response code="200">Return the api response when updated successfully</response>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [SwaggerRequestExample(typeof(DeductionUpsert), typeof(DeductionUpsertRequestExample))]
        public async Task<IActionResult> UpdateDeduction(int id, [FromBody] DeductionUpsert deductionUpdate)
        {
            return Ok(await _deductionService.UpdateDeduction(id, deductionUpdate));
        }


        /// <summary>
        /// Delete a deduction defination by id
        /// </summary>
        /// <response code="200">Return the api response when deleted successfully</response>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> RemoveDeduction(int id)
        {
            return Ok(await _deductionService.RemoveDeduction(id));
        }
    }
}
