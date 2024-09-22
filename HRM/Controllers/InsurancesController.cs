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
    [Route("api/v{v:apiVersion}/insurances")]
    [ApiController]
    [Authorize(Policy = "AdminRole")]
    public class InsurancesController : ControllerBase
    {
        private readonly IInsurancesService _insuranceService;
        public InsurancesController(IInsurancesService insuranceService)
        {
            _insuranceService = insuranceService;
        }


        /// <summary>
        /// Get all insurances in company
        /// </summary>
        /// <response code="200">Return all the insurances in the company in the metadata of api response</response>
        [HttpGet] 
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<InsuranceResult>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(InsuranceResponseExample))]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _insuranceService.GetAllInsurance());
        }


        /// <summary>
        /// Add new insurance in the company
        /// </summary>
        /// <response code="200">Return the api response</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> AddNew([FromBody] InsuranceUpsert insuranceAdd)
        {
            return Ok(await _insuranceService.AddNewInsurance(insuranceAdd));
        }


        /// <summary>
        /// Update a insurance in the company by id
        /// </summary>
        /// <response code="200">Return the api response </response>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> UpdateInsurance(int id, [FromBody] InsuranceUpsert insuranceUpdate)
        {
            return Ok(await _insuranceService.UpdateInsurance(id, insuranceUpdate));
        }


        /// <summary>
        /// Delete a insurance in the company by id
        /// </summary>
        /// <response code="200">Return the api response </response>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> RemoveInsurance(int id)
        {
            return Ok(await _insuranceService.RemoveInsurance(id));
        }
    }
}
