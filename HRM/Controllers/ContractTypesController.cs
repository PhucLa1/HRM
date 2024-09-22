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
    [Route("api/v{v:apiVersion}/ContractTypes")]
    [ApiController]
    [Authorize(Policy = "AdminRole")]
    public class ContractTypesController : ControllerBase
    {
        private readonly IContractTypesService _ContractTypeService;
        public ContractTypesController(IContractTypesService ContractTypeService)
        {
            _ContractTypeService = ContractTypeService;
        }


        /// <summary>
        /// Get all ContractTypes in company
        /// </summary>
        /// <response code="200">Return all the ContractTypes in the company in the metadata of api response</response>
        [HttpGet] 
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<ContractTypeResult>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ContractTypeResponseExample))]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _ContractTypeService.GetAllContractType());
        }


        /// <summary>
        /// Add new ContractType in the company
        /// </summary>
        /// <response code="200">Return the api response</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> AddNew([FromBody] ContractTypeUpsert ContractTypeAdd)
        {
            return Ok(await _ContractTypeService.AddNewContractType(ContractTypeAdd));
        }


        /// <summary>
        /// Update a ContractType in the company by id
        /// </summary>
        /// <response code="200">Return the api response </response>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> UpdateContractType(int id, [FromBody] ContractTypeUpsert ContractTypeUpdate)
        {
            return Ok(await _ContractTypeService.UpdateContractType(id, ContractTypeUpdate));
        }


        /// <summary>
        /// Delete a ContractType in the company by id
        /// </summary>
        /// <response code="200">Return the api response </response>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> RemoveContractType(int id)
        {
            return Ok(await _ContractTypeService.RemoveContractType(id));
        }
    }
}
