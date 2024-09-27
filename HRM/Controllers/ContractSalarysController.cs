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
    [Route("api/v{v:apiVersion}/ContractSalarys")]
    [ApiController]
    [Authorize(Policy = RoleExtensions.ADMIN_ROLE)]
    public class ContractSalarysController : ControllerBase
    {
        private readonly IContractSalarysService _ContractSalaryService;
        public ContractSalarysController(IContractSalarysService ContractSalaryService)
        {
            _ContractSalaryService = ContractSalaryService;
        }


        /// <summary>
        /// Get all ContractSalarys in company
        /// </summary>
        /// <response code="200">Return all the ContractSalarys in the company in the metadata of api response</response>
        [HttpGet] 
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<ContractSalaryResult>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ContractSalaryResponseExample))]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _ContractSalaryService.GetAllContractSalary());
        }


        /// <summary>
        /// Add new ContractSalary in the company
        /// </summary>
        /// <response code="200">Return the api response</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> AddNew([FromBody] ContractSalaryUpsert ContractSalaryAdd)
        {
            return Ok(await _ContractSalaryService.AddNewContractSalary(ContractSalaryAdd));
        }


        /// <summary>
        /// Update a ContractSalary in the company by id
        /// </summary>
        /// <response code="200">Return the api response </response>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> UpdateContractSalary(int id, [FromBody] ContractSalaryUpsert ContractSalaryUpdate)
        {
            return Ok(await _ContractSalaryService.UpdateContractSalary(id, ContractSalaryUpdate));
        }


        /// <summary>
        /// Delete a ContractSalary in the company by id
        /// </summary>
        /// <response code="200">Return the api response </response>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> RemoveContractSalary(int id)
        {
            return Ok(await _ContractSalaryService.RemoveContractSalary(id));
        }
    }
}
