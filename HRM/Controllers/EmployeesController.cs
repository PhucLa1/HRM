using Asp.Versioning;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using HRM.Services.User;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Controllers
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/employees")]
    [ApiController]
    //[Authorize(Policy = "AdminRole")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesService _employeesService;
        public EmployeesController(IEmployeesService employeesService)
        {
            _employeesService = employeesService;
        }


        /// <summary>
        /// Get all employee defination
        /// </summary>
        /// <response code="200">Return all the employee defination in the metadata of api response</response>
        [HttpGet] 
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<EmployeeResult>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ApiResponse<EmployeeResult>))]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _employeesService.GetAllEmployee());
        }


    
    }
}
