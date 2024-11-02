using Asp.Versioning;
using DocumentFormat.OpenXml.Drawing.Charts;
using HRM.Apis.Swagger.Examples.Requests;
using HRM.Apis.Swagger.Examples.Responses;
using HRM.Data.Entities;
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

        /// <summary>
        /// Update add employee in payroll by period
        /// </summary>
        /// <response code="200">Return status update true/false in the metadata of api response</response>
        [HttpPut]
        [Route("update-payroll/list")]
        public async Task<IActionResult> UpdatePayrollList([FromBody] PayrollListUpsert payrollListUpsert)
        {
            return Ok(await _payrollsService.UpdatePayroll(payrollListUpsert.Period, payrollListUpsert.EmployeeIds));
        }

        /// <summary>
        /// Update other bonus or duduction in payroll 
        /// </summary>
        /// <response code="200">Return status update true/false in the metadata of api response</response>
        [HttpPut]
        [Route("update-payroll/other-sc")]
        public async Task<IActionResult> UpdatePayrollOtherSC([FromBody] List<PayrollUpsert> payrollListUpsert)
        {
            return Ok(await _payrollsService.UpdateOtherSC(payrollListUpsert));
        }

        /// <summary>
        /// Update bonus details in payroll 
        /// </summary>
        /// <response code="200">Return status update true/false in the metadata of api response</response>
        [HttpPut]
        [Route("update-payroll/bonus-sc")]
        public async Task<IActionResult> UpdatePayrollBonusSC([FromBody] List<PayrollUpsert> payrollListUpsert)
        {
            return Ok(await _payrollsService.UpdatePayrollBonus(payrollListUpsert));
        }

        /// <summary>
        /// Update deduction details in payroll 
        /// </summary>
        /// <response code="200">Return status update true/false in the metadata of api response</response>
        [HttpPut]
        [Route("update-payroll/deduction-sc")]
        public async Task<IActionResult> UpdatePayrollDeductionSC([FromBody] List<PayrollUpsert> payrollListUpsert)
        {
            return Ok(await _payrollsService.UpdatePayrollDeduction(payrollListUpsert));
        }

        /// <summary>
        /// Update payroll formula of list employee in payroll 
        /// </summary>
        /// <response code="200">Return status update true/false in the metadata of api response</response>
        [HttpPut]
        [Route("update-payroll/formula")]
        public async Task<IActionResult> UpdatePayrollFormula([FromBody] List<PayrollUpsert> payrollListUpsert)
        {
            return Ok(await _payrollsService.UpdateFormula(payrollListUpsert));
        }


        /// <summary>
        /// Get columns dynamic for bonus / deduction
        /// </summary>
        /// <response code="200">Return columns dynamic for bonus / deduction in the metadata of api response</response>
        [HttpGet]
        [Route("salary-components/{sc_type}")]
        public async Task<IActionResult> GetDynamicColumn(int sc_type)
        {
            return Ok(await _payrollsService.GetDynamicColumn(sc_type));
        }


        /// <summary>
        /// Get table schema for payroll list
        /// </summary>
        /// <response code="200">Return columns dynamic for bunus / deduction in the metadata of api response</response>
        [HttpGet]
        [Route("table-schema/header/{year}/{month}")]
        public async Task<IActionResult> GetCompanGetPayrollTableHeaderyTree(int year, MonthPeriod month)
        {
            return Ok(await _payrollsService.GetPayrollTableHeader(new PayrollPeriod() { Month = month, Year = year}));
        }

        /// <summary>
        /// Get columns dynamic for payroll table 
        /// </summary>
        /// <response code="200">Return columns dynamic for payroll table in the metadata of api response</response>
        [HttpGet]
        [Route("table-schema/column/{year}/{month}")]
        public async Task<IActionResult> GetPayrollTableColumn(int year, MonthPeriod month)
        {
            string period = year + "/" + month;
            return Ok(await _payrollsService.GetPayrollTableColumn(new PayrollPeriod() { Month = month, Year = year }));
        }

        /// <summary>
        /// Get columns dynamic for payroll table 
        /// </summary>
        /// <response code="200">Return columns dynamic for payroll table in the metadata of api response</response>
        [HttpGet]
        [Route("{year}/{month}")]
        public async Task<IActionResult> GetPayrollTableData(int year, MonthPeriod month)
        {
            string period = year + "/" + month;
            return Ok(await _payrollsService.GetPayrollTableData(new PayrollPeriod() { Month = month, Year = year }));
        }

        /// <summary>
        /// Get employee payrol list with details sc 
        /// </summary>
        /// <response code="200">Return all employee payrol list with details sc in the metadata of api response</response>
        [HttpGet]
        [Route("employee-salary/list/{year}/{month}")]
        public async Task<IActionResult> GetListEmployeeSCByPeriod(int year, MonthPeriod month)
        {
            string period = year + "/" + month;
            return Ok(await _payrollsService.GetListEmployeeSCByPeriod(new PayrollPeriod() { Month = month, Year = year }));
        }

        /// <summary>
        /// Get employee payrol details 
        /// </summary>
        /// <response code="200">Return all employee salary components in the metadata of api response</response>
        [HttpGet]
        [Route("employee-salary/details/{payrollId}")]
        public async Task<IActionResult> GetEmployeeDetailSCByPeriod(int payrollId)
        {
            return Ok(await _payrollsService.GetEmployeeDetailSCByPeriod(payrollId));
        }
    }
}
