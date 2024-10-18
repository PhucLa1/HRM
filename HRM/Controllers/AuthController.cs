using Asp.Versioning;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using HRM.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRM.Apis.Controllers
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/auth")]
    [ApiController]
    [AllowAnonymous]
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
