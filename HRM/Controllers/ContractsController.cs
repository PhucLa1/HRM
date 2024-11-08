using Asp.Versioning;
using HRM.Data.Entities;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using HRM.Services.Briefcase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Apis.Controllers
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/contracts")]
    [ApiController]
    [Authorize(Policy = RoleExtensions.ADMIN_ROLE)]
    public class ContractsController : ControllerBase
    {
        private readonly IContractsService _contractsService;
        public ContractsController(IContractsService contractsService)
        {
            _contractsService = contractsService;
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
    }
}
