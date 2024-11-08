using Asp.Versioning;
using HRM.Apis.Swagger.Examples.Responses;
using HRM.Data.Entities;
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
    [Authorize(Policy = RoleExtensions.ADMIN_ROLE)]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentsService _departmentService;
        public DepartmentsController(IDepartmentsService departmentService)
        {
            _departmentService = departmentService;
        }

        /// <summary>
        /// Get all user name, email in department by id
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Return all the user name, email in the department in the metadata of api response</response>
        [HttpGet]
        [Route("{id}/employee")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<DepartmentUserResult>>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(DepartmentUserResponseExample))]
        public async Task<IActionResult> GetAllUserInDepartment(int id)
        {
            return Ok(await _departmentService.GetAllEmployeeInDepartment(id));
        }

        /// <summary>
        /// Get the count of employees in each department
        /// </summary>
        [HttpGet]
        [Route("employee-count")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<DepartmentEmployeeCountResult>>))]
        public async Task<IActionResult> GetEmployeeCountByDepartment()
        {
            return Ok(await _departmentService.GetEmployeeCountByDepartment());
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
