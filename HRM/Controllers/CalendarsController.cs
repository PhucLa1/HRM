using Asp.Versioning;
using HRM.Apis.Swagger.Examples.Responses;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using HRM.Services.TimeKeeping;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Controllers
{
    [Route("api/v{v:apiVersion}/calendars")]
    [ApiController]
    [ApiVersion(1)]
    [Authorize(Policy = RoleExtensions.ADMIN_ROLE)]
    public class CalendarsController : ControllerBase
    {
        private readonly ICalendarService _calendarService;
        public CalendarsController(ICalendarService calendarService)
        {
            _calendarService = calendarService;
        }


        /// <summary>
        /// This endpoint will get all calendar in database
        /// </summary>
        /// <response code="200">Return all the calendar in the company in the metadata of api response</response>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<CalendarResult>>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(CalendarResultResponseExample))]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _calendarService.GetAllCalendar());
        }


        /// <summary>
        /// This endpoint will add new work schedule
        /// </summary>
        /// <param name="calendarAdd"></param>
        /// <response code="200">Return the api response</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> AddNew([FromBody] CalendarUpsert calendarAdd)
        {
            return Ok(await _calendarService.AddNew(calendarAdd));
        }


        /// <summary>
        /// This endpoint will update a schedule by id
        /// </summary>
        /// <param name="id">The ID of the schedule to update</param>
        /// <param name="calendarUpdate">The updated schedule details</param>
        /// <response code="200">Return the api response</response>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> UpdateCalendar(int id, [FromBody] CalendarUpsert calendarUpdate)
        {
            return Ok(await _calendarService.UpdateCalendar(id, calendarUpdate));
        }


        /// <summary>
        /// This endpoint will remove a schedule by id
        /// </summary>
        /// <param name="id">The ID of the schedule to remove</param>
        /// <response code="200">Return the api response</response>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> RemoveCalendar(int id)
        {
            return Ok(await _calendarService.RemoveCalendar(id));
        }                 
    }
}
