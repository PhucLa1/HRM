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
    [Route("api/v{v:apiVersion}/departments")]
    [ApiController]
    [Authorize(Policy = "AdminRole")]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentsService _departmentService;
        public DepartmentsController(IDepartmentsService departmentService)
        {
            _departmentService = departmentService;
        }


        /// <summary>
        /// Get all departments in company
        /// </summary>
        /// <response code="200">Return all the departments in the company in the metadata of api response</response>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<DepartmentResult>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(DepartmentResponseExample))]

        public async Task<IActionResult> GetAll()
        {
            return Ok(await _departmentService.GetAllDepartment());
        }


        /// <summary>
        /// Add new department in the company
        /// </summary>
        /// <response code="200">Return the api response</response>
        [HttpPost]
        [Route("")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ApiResponse<bool>))]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> AddNew([FromBody] DepartmentUpsert departmentAdd)
        {
            return Ok(await _departmentService.AddNewDepartment(departmentAdd));
        }


        /// <summary>
        /// Update a department in the company by id
        /// </summary>
        /// <response code="200">Return the api response </response>
        [HttpPut]
        [Route("{id}")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ApiResponse<bool>))]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> UpdateDepartment(int id, [FromBody] DepartmentUpsert departmentUpdate)
        {
            return Ok(await _departmentService.UpdateDepartment(id, departmentUpdate));
        }


        /// <summary>
        /// Delete a department in the company by id
        /// </summary>
        /// <response code="200">Return the api response </response>
        [HttpDelete]
        [Route("{id}")]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ApiResponse<bool>))]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> RemoveDepartment(int id)
        {
            return Ok(await _departmentService.RemoveDepartment(id));
        }
    }
    
}
