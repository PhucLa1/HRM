using Asp.Versioning;
using HRM.Apis.Swagger.Examples.Requests;
using HRM.Apis.Swagger.Examples.Responses;
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
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesService _employeesService;
        public EmployeesController(IEmployeesService employeesService)
        {
            _employeesService = employeesService;
        }


        /// <summary>
        /// Add face regis to employee to use face recognition
        /// </summary>
        /// <response code="200">Return the api response</response>
        [HttpPost]
        [Route("regis-face/{id}")]
        public async Task<IActionResult> RegistrationFace(int id, [FromForm] List<FaceRegis> faceRegises)
        {
            return Ok(await _employeesService.RegistrationFace(id, faceRegises));
        }


        /// <summary>
        /// Get all face regis in company by employee id
        /// </summary>
        /// <response code="200">Return all the  face regis in company by employee id in the metadata of api response</response>
        [HttpGet]
        [Route("{id}/employee-images")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<FaceRegisResult>>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(FaceRegisResultResponseExample))]
        public async Task<IActionResult> GetAllFaceRegisByEmployeeId(int id)
        {
            return Ok(await _employeesService.GetAllFaceRegisByEmployeeId(id));
        }
        
        
        /// <summary>
        /// Get all employee defination
        /// </summary>
        /// <response code="200">Return all the employee defination in the metadata of api response</response>
        [HttpGet] 
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<EmployeeResult>>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ApiResponse<List<EmployeeResult>>))]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _employeesService.GetAllEmployee());
        }

        /// <summary>
        /// Get employee infor by contract id defination
        /// </summary>
        /// <response code="200">Return selected employee by contract id defination in the metadata of api response</response>
        [HttpGet]
        [Route("contract/{contractId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<EmployeeResult>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ApiResponse<EmployeeResult>))]
        public async Task<IActionResult> GetEmployeeByContractId(int contractId)
        {
            return Ok(await _employeesService.GetEmployeeInfoByContract(contractId));
        }

        /// <summary>
        /// Get list contract id not in used defination
        /// </summary>
        /// <response code="200">Return contract id not in used defination in the metadata of api response</response>
        [HttpGet]
        [Route("contract/not-inused")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<int>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ApiResponse<int>))]
        public async Task<IActionResult> GetListContractId()
        {
            return Ok(await _employeesService.GetContractIdsNotInUsed());
        }



        /// <summary>
        /// Add new employee defination
        /// </summary>
        /// <response code="200">Return the api response when inserted succesfully</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [SwaggerRequestExample(typeof(EmployeeUpsert), typeof(EmployeeUpsert))]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeUpsert employeeUpsert)
        {
            return Ok(await _employeesService.AddEmployee(employeeUpsert));
        }

        /// <summary>
        /// Update a employee defination by id
        /// </summary>
        /// <response code="200">Return the api response when updated succesfully</response>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [SwaggerRequestExample(typeof(EmployeeUpsert), typeof(EmployeeUpsert))]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] EmployeeUpsert employeeUpsert)
        {
            return Ok(await _employeesService.UpdateEmployee(id, employeeUpsert));
        }

        /// <summary>
        /// Delete a employee defination by id
        /// </summary>
        /// <response code="200">Return the api response when deleted succesfully</response>
        [HttpDelete]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        public async Task<IActionResult> RemoveEmployee(int id)
        {
            return Ok(await _employeesService.RemoveEmployee(id));
        }


        /// <summary>
        /// Send login information to employee defination
        /// </summary>
        /// <response code="200">Return the api response when sent succesfully</response>
        [HttpPost]
        [Route("send-login-info")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<bool>))]
        [SwaggerRequestExample(typeof(EmployeeUpsert), typeof(EmployeeUpsert))]
        public async Task<IActionResult> SendLoginInfo([FromBody] EmployeeUpsert employeeUpsert)
        {
            return Ok(await _employeesService.SendEmailToEmployee(employeeUpsert));
        }

        /// <summary>
        /// Get current profile of current user
        /// </summary>
        /// <response code="200">Return the current user profile defination in the metadata of api response</response>
        [HttpGet]
        [Route("current-profile")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<ProfileDetail>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(ProfileDetailResponseExample))]
        public async Task<IActionResult> GetCurrentUserProfile()
        {
            return Ok(await _employeesService.GetCurrentProfileUser());
        }


        /// <summary>
        /// Update face regis to employee to use face recognition
        /// </summary>
        /// <response code="200">Return the api response</response>
        [HttpPut]
        [Route("update-regis-face/{id}")]
        public async Task<IActionResult> RegistrationFace(int id, [FromForm] List<FaceRegisUpdate> faceRegisUpdates)
        {
            return Ok(await _employeesService.UpdateFaceRegis(id, faceRegisUpdates));
        }


        /// <summary>
        /// Get all employee label description
        /// </summary>
        /// <response code="200">Return all the employee label description in the metadata of api response</response>
        [HttpGet]
        [Route("get-all-labeled")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<List<LabelDescriptions>>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(LabelDescriptionsResponseExample))]
        public async Task<IActionResult> GetAllLabelDescription()
        {
            return Ok(await _employeesService.GetAllLabelDescription());
        }
    }
}


        

