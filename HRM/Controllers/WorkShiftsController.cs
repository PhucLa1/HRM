using Asp.Versioning;
using HRM.Apis.Swagger.Examples.Responses;
using HRM.Data.Entities;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using HRM.Services.TimeKeeping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Controllers
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/work-shifts")]
    [ApiController]
    
    
    public class WorkShiftsController : ControllerBase
    {
        private readonly IWorkShiftService _workShiftService;
        public WorkShiftsController(IWorkShiftService workShiftService)
        {
            _workShiftService = workShiftService;
        }

        /// <summary>
        /// Register shift from employee
        /// </summary>
        /// <response code="200">Return the api response</response>
        [HttpPost]
        [Route("register-shift")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [Authorize(Policy = RoleExtensions.USER_ROLE)]
        public async Task<IActionResult> RegisterShift([FromBody] WorkPlanInsert workPlanInsert)
        {
            return Ok(await _workShiftService.RegisterWorkShift(workPlanInsert));
        }

        /// <summary>
        /// Get all partime plans from employee
        /// </summary>
        /// <response code="200">Return the api response</response>
        [HttpGet]
        [Route("get-all-partimeplans")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<PartimePlanResult>>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(PartimePlanResultResponseExample))]
        public async Task<IActionResult> GetAllPartimePlan()
        {
            return Ok(await _workShiftService.GetAllPartimePlan());
        }

        /// <summary>
        /// Get  partime plans by id from employee
        /// </summary>
        /// <response code="200">Return the api response</response>
        [HttpGet]
        [Route("get-partimeplan/{partimePlanId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<PartimePlanResult>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(PartimePlanResultDetailResponseExample))]
        public async Task<IActionResult> GetPartimePlanById(int partimePlanId)
        {
            return Ok(await _workShiftService.GetDetailPartimePlan(partimePlanId));
        }

        /// <summary>
        /// Get all work shift from partime plan
        /// </summary>
        /// <response code="200">Return the api response</response>
        [HttpGet]
        [Route("get-all-workshifts/{partimePlanId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<UserCalendarResult>>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(UserCalendarResultResponseExample))]
        public async Task<IActionResult> GetAllWorkShiftsByPartimePlanId(int partimePlanId)
        {
            return Ok(await _workShiftService.GetAllWorkShiftByPartimePlanId(partimePlanId));
        }


        /// <summary>
        /// Process request of partime plan from employee
        /// </summary>
        /// <response code="200">Return the api response</response>
        [HttpPut]
        [Route("process-partimeplan/{partimePlanId}")]
        public async Task<IActionResult> ProcessPartimePlanRequest(int partimePlanId, StatusCalendar statusCalendar)
        {
            return Ok(await _workShiftService.ProcessPartimePlanRequest(partimePlanId,statusCalendar));
        }
    }
}
