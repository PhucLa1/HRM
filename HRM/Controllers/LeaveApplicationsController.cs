using Asp.Versioning;
using HRM.Apis.Swagger.Examples.Requests;
using HRM.Apis.Swagger.Examples.Responses;
using HRM.Data.Entities;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using HRM.Services.Briefcase;
using HRM.Services.TimeKeeping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Controllers
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/leave-applications")]
    [ApiController]
    [Authorize(Policy = RoleExtensions.ADMIN_ROLE)]
    public class LeaveApplicationsController : ControllerBase
    {
        private readonly ILeaveApplicationsService _leaveApplicationsService;
        public LeaveApplicationsController(ILeaveApplicationsService leaveApplicationsService)
        {
            _leaveApplicationsService = leaveApplicationsService;
        }


        /// <summary>
        /// Get all leave applications in company
        /// </summary>
        /// <response code="200">Return all the applications in the company in the metadata of api response</response>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<LeaveApplicationResult>>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(LeaveApplicationResponseExample))]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _leaveApplicationsService.GetAllApplications());
        }

        /// Get all user name, email in department by id
        /// </summary>
        /// <param name="id"></param>
        /// <response code="200">Return all the user name, email in the department in the metadata of api response</response>
        [HttpGet]
        [Route("employee")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<EmployeeDataResult>>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(LeaveApplicationResponseExample))]
        public async Task<IActionResult> GetAllEmployee()
        {
            return Ok(await _leaveApplicationsService.GetAllEmployees());
        }

        /// <summary>
        /// Add new leave application in the company
        /// </summary>
        /// <response code="200">Return the api response</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [SwaggerRequestExample(typeof(LeaveApplicationUpSert), typeof(LeaveApplicationUpsertRequestExample))]

        public async Task<IActionResult> AddNew([FromBody] LeaveApplicationUpSert leaveAdd)
        {
            return Ok(await _leaveApplicationsService.AddNewApplication(leaveAdd));
        }


        /// <summary>
        /// Update a leave application in the company by id
        /// </summary>
        /// <response code="200">Return the api response </response>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [SwaggerRequestExample(typeof(LeaveApplicationUpSert), typeof(LeaveApplicationUpsertRequestExample))]

        public async Task<IActionResult> UpdatePosition(int id, [FromBody] LeaveApplicationUpSert leaveUpdate)
        {
            return Ok(await _leaveApplicationsService.UpdateApplication(id, leaveUpdate));
        }


        /// <summary>
        /// Delete a leave application in the company by id
        /// </summary>
        /// <response code="200">Return the api response </response>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> RemovePosition(int id)
        {
            return Ok(await _leaveApplicationsService.RemoveApplication(id));
        }
    }
}
