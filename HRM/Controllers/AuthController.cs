using Asp.Versioning;
using HRM.Apis.Swagger.Examples.Requests;
using HRM.Apis.Swagger.Examples.Responses;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using HRM.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Controllers
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        /// <summary>
        /// This endpoint is login in page admin 
        /// </summary>
        /// <param name="adminLogin"></param>
        /// <returns>Return the api response , the message is the error of api return</returns>
        /// <response code="200">Return the api response</response>
        [HttpPost]
        [Route("admin-login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<string>))]
        public async Task<IActionResult> Login([FromBody] AccountLogin adminLogin)
        {
            var result = await _authService.AdminLogin(adminLogin);
            if (result.Metadata != null)
            {
                SetJWT(result.Metadata);
            }
            return Ok(result);
        }

        /// <summary>
        /// This endpoint is login in page user 
        /// </summary>
        /// <param name="employeeLogin"></param>
        /// <returns>Return the api response , the message is the error of api return</returns>
        /// <response code="200">Return the api response</response>
        [HttpPost]
        [Route("employee-login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<string>))]
        public async Task<IActionResult> EmployeeLogin([FromBody] AccountLogin employeeLogin)
        {
            var result = await _authService.EmployeeLogin(employeeLogin);
            if (result.Metadata != null)
            {
                SetJWT(result.Metadata);
            }
            return Ok(result);
        }


        /// <summary>
        /// This endpoint is login by QR in page user 
        /// </summary>
        /// <param name="employeeLogin"></param>
        /// <returns>Return the api response , the message is the error of api return</returns>
        /// <response code="200">Return the api response</response>
        [HttpPost]
        [Route("employee-login-qr")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<string>))]
        public async Task<IActionResult> EmployeeLoginByQr([FromBody] AccountLogin employeeLogin)
        {
            var result = await _authService.EmployeeLoginByQr(employeeLogin);
            if (result.Metadata != null)
            {
                SetJWT(result.Metadata);
            }
            return Ok(result);
        }




        /// <summary>
        /// This endpoint get the info of current user
        /// </summary>
        /// /// <remarks>
        /// This endpoint allows clients to fetch details about the logged-in user, 
        /// including user profile information, preferences, and any relevant settings. 
        /// It is useful for personalizing user experience and managing user-specific data.
        /// <para> 
        /// About Role : 
        /// </para>
        /// <para>
        /// 1 : Admin
        /// </para>
        /// <para>
        /// 2 : Partime
        /// </para>
        /// <para> 
        /// 3 : FullTime 
        /// </para>
        /// </remarks>
        /// <response code="200">Return all the bonus defination in the metadata of api response</response>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<AccountInfo>))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(AccountInfoResponseExample))]
        [HttpGet]
        [Route("get-current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            return Ok(await _authService.GetCurrentAccount());
        }


        /// <summary>
        /// This endpoint will remove the jwt token and log out the web
        /// </summary>
        /// <returns>Return the api response with T is bool</returns>
        [HttpDelete]
        [Route("log-out")]
        public async Task<ActionResult> SignOut()
        {
            DeleteJWT();
            return Ok(new ApiResponse<bool> { IsSuccess = true });
        }



        /// <summary>
        /// This endpoint will update the information of account user
        /// </summary>
        /// <returns>Return the api response with T is bool</returns>
        [HttpPut]
        [Route("update-account/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<string>))]
        [SwaggerRequestExample(typeof(AccountUpdate), typeof(AccountUpdateRequestExample))]
        public async Task<ActionResult> UpdateAccountInformation(int id, [FromBody] AccountUpdate accountUpdate)
        {
            return Ok(await _authService.ChangeAccountInformation(id, accountUpdate));
        }





        private void SetJWT(string encryptedToken)
        {
            HttpContext.Response.Cookies.Append("X-Access-Token", encryptedToken,
                 new CookieOptions
                 {
                     Expires = DateTime.UtcNow.AddDays(15),
                     HttpOnly = true,
                     Secure = true,
                     IsEssential = true,
                     SameSite = SameSiteMode.None
                 });
        }

        private void DeleteJWT()
        {
            HttpContext.Response.Cookies.Delete("X-Access-Token");
        }
    }
}
