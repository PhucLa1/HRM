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
    [Route("api/v{v:apiVersion}/tax-rates")]
    [ApiController]
    //[Authorize(Policy = "AdminRole")]
    public class TaxRatesController : ControllerBase
    {
        private readonly ITaxRatesService _taxRatesService;
        public TaxRatesController(ITaxRatesService taxRatesService)
        {
            _taxRatesService = taxRatesService;
        }


        /// <summary>
        /// Get all Tax Rate defination
        /// </summary>
        /// <response code="200">Return all the taxRates defination in the metadata of api response</response>
        [HttpGet] 
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<TaxRateResult>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TaxRateResponseExample))]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _taxRatesService.GetAllTaxRate());
        }


        /// <summary>
        /// Add new Tax Rate defination
        /// </summary>
        /// <response code="200">Return the api response when inserted succesfully</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [SwaggerRequestExample(typeof(TaxRateUpsert), typeof(TaxRateUpsertRequestExample))]
        public async Task<IActionResult> AddNew([FromBody] TaxRateUpsert taxRatesAdd)
        {
            return Ok(await _taxRatesService.AddNewTaxRate(taxRatesAdd));
        }


        /// <summary>
        /// Update a Tax Rate defination by id
        /// </summary>
        /// <response code="200">Return the api response when updated succesfully</response>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [SwaggerRequestExample(typeof(TaxRateUpsert), typeof(TaxRateUpsertRequestExample))]
        public async Task<IActionResult> UpdateTaxRate(int id, [FromBody] TaxRateUpsert taxRatesUpdate)
        {
            return Ok(await _taxRatesService.UpdateTaxRate(id, taxRatesUpdate));
        }


        /// <summary>
        /// Delete a Tax Rate defination by id
        /// </summary>
        /// <response code="200">Return the api response when deleted succesfully</response>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> RemoveTaxRate(int id)
        {
            return Ok(await _taxRatesService.RemoveTaxRate(id));
        }
    }
}
