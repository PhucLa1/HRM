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
    [Route("api/v{v:apiVersion}/payrolls")]
    [ApiController]
    //[Authorize(Policy = "AdminRole")]
    public class PayrollsController : ControllerBase
    {
        private readonly IPayrollsService _payrollsService;
        public PayrollsController(IPayrollsService payrollsService)
        {
            _payrollsService = payrollsService;
        }


        /// <summary>
        /// Get company tree
        /// </summary>
        /// <response code="200">Return company tree department->position->employee in the metadata of api response</response>
        [HttpGet] 
        [Route("company-tree")]
        public async Task<IActionResult> GetCompanyTree()
        {
            return Ok(await _payrollsService.GetCompanyTree());
        }
    }
}
