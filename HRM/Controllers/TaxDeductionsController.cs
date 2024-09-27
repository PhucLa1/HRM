using Asp.Versioning;
using HRM.Apis.Swagger.Examples.Requests;
using HRM.Apis.Swagger.Examples.Responses;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using HRM.Services.Salary;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Controllers
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/tax-deductions")]
    [ApiController]
    //[Authorize(Policy = "AdminRole")]
    public class TaxDeductionsController : ControllerBase
    {
        private readonly ITaxDeductionsService _taxDeductionsService;
        public TaxDeductionsController(ITaxDeductionsService taxDeductionsService)
        {
            _taxDeductionsService = taxDeductionsService;
        }


        /// <summary>
        /// Get all Tax Deduction defination
        /// </summary>
        /// <response code="200">Return all the taxDeductions defination in the metadata of api response</response>
        [HttpGet] 
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<TaxDeductionResult>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(TaxDeductionResponseExample))]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _taxDeductionsService.GetAllTaxDeduction());
        }


        /// <summary>
        /// Add new Tax Deduction defination
        /// </summary>
        /// <response code="200">Return the api response when inserted succesfully</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [SwaggerRequestExample(typeof(TaxDeductionUpsert), typeof(TaxDeductionUpsertRequestExample))]
        public async Task<IActionResult> AddNew([FromBody] TaxDeductionUpsert taxDeductionsAdd)
        {
            return Ok(await _taxDeductionsService.AddNewTaxDeduction(taxDeductionsAdd));
        }


        /// <summary>
        /// Update a Tax Deduction defination by id
        /// </summary>
        /// <response code="200">Return the api response when updated succesfully</response>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [SwaggerRequestExample(typeof(TaxDeductionUpsert), typeof(TaxDeductionUpsertRequestExample))]
        public async Task<IActionResult> UpdateTaxDeductions(int id, [FromBody] TaxDeductionUpsert taxDeductionsUpdate)
        {
            return Ok(await _taxDeductionsService.UpdateTaxDeduction(id, taxDeductionsUpdate));
        }


        /// <summary>
        /// Delete a Tax Deduction defination by id
        /// </summary>
        /// <response code="200">Return the api response when deleted succesfully</response>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> RemoveTaxDeductions(int id)
        {
            return Ok(await _taxDeductionsService.RemoveTaxDeduction(id));
        }
    }
}
