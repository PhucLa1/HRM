﻿using Asp.Versioning;
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
        /// Get test
        /// </summary>
        /// <response code="200">Return test in the metadata of api response</response>
        [HttpGet]
        [Route("test")]
        public async Task<IActionResult> GetTest()
        {
            return Ok(await _payrollsService.Test());
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
        /// Update specific payroll of an employee in period
        /// </summary>
        /// <response code="200">Return status update true/false in the metadata of api response</response>
        [HttpPut]
        [Route("update-payroll/details")]
        public async Task<IActionResult> UpdatePayrollDetails([FromBody] PayrollUpsert payrollUpsert)
        {
            return Ok(await _payrollsService.UpdatePayrollDetails(payrollUpsert));
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
        /// Get all history saving of payroll
        /// </summary>
        /// <response code="200">Return all history saving of payrollin the metadata of api response</response>
        [HttpGet]
        [Route("table-schema/history")]
        public async Task<IActionResult> GetAllPayrollHistory()
        {
            return Ok(await _payrollsService.GetAllPayrollHistory());
        }

        /// <summary>
        /// Get history saving of payroll bt id (details)
        /// </summary>
        /// <response code="200">Return history saving of payroll by hisID in the metadata of api response</response>
        [HttpGet]
        [Route("table-schema/history/{payrollHistoryId}")]
        public async Task<IActionResult> GetPayrollHistoryDetails(int payrollHistoryId)
        {
            return Ok(await _payrollsService.GetPayrollHistoryDetails(payrollHistoryId));
        }

        /// <summary>
        /// Add history
        /// </summary>
        /// <response code="200">Return status of  saving in the metadata of api response</response>
        [HttpPost]
        [Route("table-schema/history")]
        public async Task<IActionResult> SavePayrollHistory(PayrollHistoryModel payrollHistory)
        {
            return Ok(await _payrollsService.SavePayrollHistory(payrollHistory));
        }

        /// <summary>
        /// Get history saving of payroll bt id (details)
        /// </summary>
        /// <response code="200">Return history deleting of payroll by hisID in the metadata of api response</response>
        [HttpDelete]
        [Route("table-schema/history/{payrollHistoryId}")]
        public async Task<IActionResult> RemovePayrollHistory(int payrollHistoryId)
        {
            return Ok(await _payrollsService.RemovePayrollHistory(payrollHistoryId));
        }


        /// <summary>
        /// Get columns dynamic for payroll table 
        /// </summary>
        /// <response code="200">Return columns dynamic for payroll table in the metadata of api response</response>
        [HttpPost]
        [Route("{year}/{month}")]
        public async Task<IActionResult> GetPayrollTableData(int year, MonthPeriod month, [FromBody] PayrollFilter filter)
        {
            string period = year + "/" + month;
            return Ok(await _payrollsService.GetPayrollTableData(new PayrollPeriod() { Month = month, Year = year }, filter.Dfrom, filter.Dto));
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

        /// <summary>
        /// Send payslip for list employee in specific payroll
        /// </summary>
        /// <response code="200">Return status of email forwarding in the metadata of api response</response>
        [HttpPost]
        [Route("employee-salary/payslip/{year}/{month}")]
        public async Task<IActionResult> GetListEmployeeSCByPeriod(int year, MonthPeriod month, [FromBody] PayrollFilter filter)
        {
            return Ok(await _payrollsService.SendPayslip(new PayrollPeriod() { Month = month, Year = year }, filter.Dfrom, filter.Dto, filter.EmployeeIds));
        }
    }
}
