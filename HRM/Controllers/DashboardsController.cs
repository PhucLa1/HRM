using HRM.Services.Dashboard;
using HRM.Repositories.Dtos.Results;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using HRM.Data.Entities;
using HRM.Repositories.Dtos.Models;

namespace HRM.Controllers
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/dashboards")]
    [ApiController]
    [Authorize(Policy = RoleExtensions.ADMIN_ROLE)]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardsService _dashboardsService;
        public DashboardController(IDashboardsService dashboardsService)
        {
            _dashboardsService = dashboardsService;
        }

        /// <summary>
        /// Get all employee count by base salary
        /// </summary>
        /// <response code="200">Return all employee count by base salary in the metadata of api response</response>
        [HttpGet("employee-count-by-base-salary")]
        public async Task<IActionResult> GetEmployeeCountByBaseSalary()
        {
            return Ok(await _dashboardsService.GetEmployeeCountByBaseSalary());
        }


        /// <summary>
        /// Get all page flexible dashboard
        /// </summary>
        /// <response code="200">Returnall page flexible dashboard in the metadata of api response</response>
        [HttpGet("all-flexible-dashboards")]
        public async Task<IActionResult> GetAllFlexibleDashboard()
        {
            return Ok(await _dashboardsService.GetAllFlexibleDashboard());
        }

        /// <summary>
        /// Add new flexible dashboard
        /// </summary>
        [HttpPost("create-flexible-dashboards")]
        public async Task<IActionResult> CreateNewPageFlexibleDashboard()
        {
            return Ok(await _dashboardsService.CreateNewPageFlexibleDashboard());
        }


        /// <summary>
        /// Add new chart in flexible dashboard
        /// </summary>
        [HttpPost("charts")]
        public async Task<IActionResult> CreateNewChart([FromBody] ChartUpsert chartAdd)
        {
            return Ok(await _dashboardsService.CreateNewChart(chartAdd));
        }


        /// <summary>
        /// Get all charts by page id
        /// </summary>
        /// <response code="200">Return all charts by page id in the metadata of api response</response>
        [HttpGet("{pageId}/charts")]
        public async Task<IActionResult> GetAllChartByPageFlexibleId(int pageId)
        {
            return Ok(await _dashboardsService.GetAllChartByPageFlexibleId(pageId));
        }



        /// <summary>
        /// Get job posting count
        /// </summary>
        /// <response code="200">Return number of job posting in the metadata of api response</response>
        [HttpGet("job-posting-count")]
        public async Task<IActionResult> GetJobPostingCount()
        {
            return Ok(await _dashboardsService.GetJobPostingCount());
        }



        /// <summary>
        /// Get all applicant count by position
        /// </summary>
        /// <response code="200">Return all applicant count by position in the metadata of api response</response>
        [HttpGet("applicant-count-by-position")]
        public async Task<IActionResult> GetApplicantCountByPosition()
        {
            return Ok(await _dashboardsService.GetApplicantForPosition());
        }





        /// <summary>
        /// Get applicant count
        /// </summary>
        /// <response code="200">Return number of applicant in the metadata of api response</response>
        [HttpGet("applicant-count")]
        public async Task<IActionResult> GetApplicantCount()
        {
            return Ok(await _dashboardsService.GetApplicantCount());
        }

        /// <summary>
        /// Get all leave application today
        /// </summary>
        /// <response code="200">Return all leave application today in the metadata of api response</response>
        [HttpGet("leave-applications-today")]
        public async Task<IActionResult> GetLeaveApplicationsToday()
        {
            return Ok(await _dashboardsService.GetLeaveApplicationsToday());
        }

        /// <summary>
        /// Get all contract before expiring date 30 days 
        /// </summary>
        /// <response code="200">Return all contract before expiring date 30 days in the metadata of api response</response>
        [HttpGet("expiring-contracts")]
        public async Task<IActionResult> GetExpiringContracts(string expirationDate)
        {
            return Ok(await _dashboardsService.GetExpiringContracts(expirationDate));
        }

        /// <summary>
        /// Get all advances during pay period 
        /// </summary>
        /// <response code="200">Return all advance during pay period in the metadata of api response</response>
        [HttpGet("advances-by-pay-period")]
        public async Task<IActionResult> GetAdvancesByPayPeriod(string start, string end)
        {
            return Ok(await _dashboardsService.GetAdvanceCountByPeriod(start, end));
        }



        [HttpGet("flexible-dashboard")]
        public async Task<IActionResult> FlexibleDashboard()
        {
            return Ok(await _dashboardsService.FlexibleDashboard());
        }
    }
}