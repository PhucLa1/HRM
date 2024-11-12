using Asp.Versioning;
using HRM.Apis.Swagger.Examples.Requests;
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
        [Authorize(Policy = RoleExtensions.PARTIME_ROLE)]
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
        /// Get all partime plans by current partime employee 
        /// </summary>
        /// <response code="200">Return the api response</response>
        [HttpGet]
        [Route("get-partimeplans-by-current-employee")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<PartimePlanResult>>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(PartimePlanResultResponseExample))]
        public async Task<IActionResult> GetAllPartimePlanByCurrentEmployeeId()
        {
            return Ok(await _workShiftService.GetAllPartimePlanByCurrentEmployeeId());
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
        /// Get all work shift from partime plan - (yyyy-mm-dd)
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
            return Ok(await _workShiftService.ProcessPartimePlanRequest(partimePlanId, statusCalendar));
        }



        /// <summary>
        /// Get all work shift by employee id - (yyyy-mm-dd)
        /// </summary>
        /// <response code="200">Return the api response</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<List<CalendarEntry>>>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(CalendarEntryResponseExample))]
        [HttpGet]
        [Route("get-all-user-calendar-by-employee/{employeeId}")]
        public async Task<IActionResult> GetAllWorkShiftByEmployee(int employeeId, string startDate, string endDate)
        {
            return Ok(await _workShiftService.GetAllWorkShiftByPartimeEmployee(employeeId, startDate, endDate));
        }

        /// <summary>
        /// Get all history result by employee id - (yyyy-mm-dd) - fulltime
        /// </summary>
        /// <response code="200">Return the api response</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<EmployeeAttendanceRecord>>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(EmployeeAttendanceRecordsResponseExample))]
        [HttpGet]
        [Route("get-all-attendance-by-fulltime-employee/{employeeId}")]
        public async Task<IActionResult> GetAllWorkShiftByFullTimeEmployee(int employeeId, string startDate, string endDate)
        {
            return Ok(await _workShiftService.GetAllWorkShiftByFullTimeEmployee(employeeId, startDate, endDate));
        }


        /// <summary>
        /// Print work shift of partime employee to excel
        /// </summary>
        /// <response code="200">Return the api response</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [HttpPost]
        [Route("print-partime-work-shift-to-excel/{employeeId}")]
        public async Task<IActionResult> PrintWorkShiftToExcel(int employeeId, string startDate, string endDate)
        {
            return Ok(await _workShiftService.PrintPartimeWorkShiftToExcel(employeeId, startDate, endDate));
        }



        /// <summary>
        /// Print history attendance of fulltime employee to excel
        /// </summary>
        /// <response code="200">Return the api response</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [HttpPost]
        [Route("print-fulltime-attendance-to-excel/{employeeId}")]
        public async Task<IActionResult> PrintFullTimeAttendanceToExcel(int employeeId, string startDate, string endDate)
        {
            return Ok(await _workShiftService.PrintFullTimeAttendanceToExcel(employeeId, startDate, endDate));
        }


        //History of attendance tracking

        /// <summary>
        /// Check in out of employee
        /// </summary>
        /// <response code="200">Return the api response</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [SwaggerRequestExample(typeof(HistoryUpsert), typeof(HistoryUpsertRequestExample))]
        [HttpPost]
        [Route("check-in-out-employee/{employeeId}")]
        public async Task<IActionResult> PrintWorkShiftToExcel(int employeeId, [FromBody] HistoryUpsert historyAdd)
        {
            return Ok(await _workShiftService.CheckInOutEmployee(employeeId, historyAdd));
        }



        /// <summary>
        /// Update the attendance tracking of employee
        /// </summary>
        /// <response code="200">Return the api response</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [SwaggerRequestExample(typeof(HistoryUpsert), typeof(HistoryUpsertRequestExample))]
        [HttpPut]
        [Route("history/{id}")]
        public async Task<IActionResult> UpdateHistoryAttendance(int id, [FromBody] HistoryUpsert historyUpdate)
        {
            return Ok(await _workShiftService.UpdateHistoryAttendance(id, historyUpdate));
        }


    }
}
