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
    [Route("api/v{v:apiVersion}/fomulas")]
    [ApiController]
    //[Authorize(Policy = "AdminRole")]
    public class FomulasController : ControllerBase
    {
        private readonly IFomulasService _fomulaService;
        public FomulasController(IFomulasService fomulaService)
        {
            _fomulaService = fomulaService;
        }


        /// <summary>
        /// Get all fomulas defination
        /// </summary>
        /// <response code="200">Return all the fomulas defination in the metadata of api response</response>
        [HttpGet] 
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<FomulaResult>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(FomulaResponseExample))]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _fomulaService.GetAllFomula());
        }


        /// <summary>
        /// Add new fomula defination
        /// </summary>
        /// <response code="200">Return the api response when inserted successfully</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [SwaggerRequestExample(typeof(FomulaUpsert), typeof(FomulaUpsertRequestExample))]
        public async Task<IActionResult> AddNew([FromBody] FomulaUpsert fomulaAdd)
        {
            return Ok(await _fomulaService.AddNewFomula(fomulaAdd));
        }


        /// <summary>
        /// Update a fomula defination by id
        /// </summary>
        /// <response code="200">Return the api response when updated successfully</response>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [SwaggerRequestExample(typeof(FomulaUpsert), typeof(FomulaUpsertRequestExample))]
        public async Task<IActionResult> UpdateFomula(int id, [FromBody] FomulaUpsert fomulaUpdate)
        {
            return Ok(await _fomulaService.UpdateFomula(id, fomulaUpdate));
        }


        /// <summary>
        /// Delete a fomula defination by id
        /// </summary>
        /// <response code="200">Return the api response when deleted successfully</response>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> RemoveFomula(int id)
        {
            return Ok(await _fomulaService.RemoveFomula(id));
        }


        /// <summary>
        /// Get all Salary Components
        /// </summary>
        /// <response code="200">Return all the  Salary Components defination in the metadata of api response</response>
        [HttpGet]
        [Route("salary-components")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<IEnumerable<SalaryComponentCategory>>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ApiResponse<IEnumerable<SalaryComponentCategory>>))]
        public async Task<IActionResult> GetAllSalaryComponents()
        {
            return Ok(await _fomulaService.GetAllSalaryComponents());
        }
    }
}
