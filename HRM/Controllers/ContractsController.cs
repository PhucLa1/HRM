using Asp.Versioning;
using HRM.Data.Entities;
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
    [Route("api/v{v:apiVersion}/contracts")]
    [ApiController]
    //[Authorize(Policy = RoleExtensions.ADMIN_ROLE)]
    public class ContractsController : ControllerBase
    {
        private readonly IContractsService _contractsService;
        public ContractsController(IContractsService contractsService)
        {
            _contractsService = contractsService;
        }

        /// <summary>
        /// Get all Contracts in company
        /// </summary>
        /// <response code="200">Return all the Contracts in the company in the metadata of api response</response>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<ContractResult>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ContractResponseExample))]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _contractsService.GetAllContract());
        }

        /// <summary>
        /// Add new Contracts in the company when HR create contracts
        /// </summary>
        /// <response code="200">Return the api response</response>
        [HttpPost]
        [Route("applicants/{applicantId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> AddNewContract(int applicantId, [FromBody] ContractAdd contractAdd)
        {
            return Ok(await _contractsService.CreateNewContract(applicantId, contractAdd));
        }

        /// <summary>
        /// Add new Contracts by HR
        /// </summary>
        /// <response code="200">Return the api response</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> AddNew([FromBody] ContractUpsert contractAdd)
        {
            return Ok(await _contractsService.AddNewContract(contractAdd));
        }

        /// <summary>
        /// This endpoint will update a contract by id when applicants fill form
        /// </summary>
        /// <param name="id">The ID of the contract to update</param>
        /// <param name="contractUpdate">The contract schedule details</param>
        /// <response code="200">Return the api response</response>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> FillContractDetail(int id, [FromBody] ContractUpdate contractUpdate)
        {
            return Ok(await _contractsService.FillContractDetails(id, contractUpdate));
        }

        /// <summary>
        /// Update the whole contract?
        /// </summary>
        /// <response code="200">Return the api response when updated succesfully</response>
        [HttpPut]
        [Route("update-contract/{id}")]
        public async Task<IActionResult> UpdateContract(int id, [FromBody] ContractUpsert contractUpdate)
        {
            return Ok(await _contractsService.UpdateContract(id, contractUpdate));
        }

        /// <summary>
        /// Update advance status by id: just for role admin
        /// </summary>
        /// <response code="200">Return the api response when updated succesfully</response>
        [HttpPut]
        [Route("update-status/{id}/{status}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [SwaggerRequestExample(typeof(AdvanceUpsert), typeof(AdvanceUpsert))]
        public async Task<IActionResult> UpdateContractStatus(int id, ContractStatus status)
        {
            return Ok(await _contractsService.UpdateContractStatus(id, status));
        }
        
        /// <summary>
        /// Delete a Contract in the company by id
        /// </summary>
        /// <response code="200">Return the api response </response>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> RemoveContract(int id)
        {
            return Ok(await _contractsService.RemoveContract(id));
        }

        /// <summary>
        /// Sign contract by employee
        /// </summary>
        /// <response code="200">Return status after signing the api response</response>
        [HttpPost]
        [Route("add-employee-signature/{contractId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> AddEmployeeSignature(int contractId, [FromForm] DigitalSignature signatureModel)
        {
            return Ok(await _contractsService.SignContract(contractId, signatureModel));
        }

        /// <summary>
        /// Sign contract by employee
        /// </summary>
        /// <response code="200">Return status after signing the api response</response>
        //[Authorize(Policy = RoleExtensions.FULLTIME_ROLE||Policy = RoleExtensions.PARTIME_ROLE)]
        //[AllowAnonymous]
        [HttpPost]
        [Route("create-contract-pdf/{contractId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> GenerateContractPDF(int contractId)
        {
            return Ok(await _contractsService.GenerateContractPDF(contractId));
        }
    }
}
