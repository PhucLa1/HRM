using FluentValidation;
using HRM.Apis.Setting;
using HRM.Data.Entities;
using HRM.Repositories.Base;
using HRM.Repositories.Dtos.Models;
using HRM.Repositories.Dtos.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace HRM.Services.User
{
    public static class AuthError
    {
        public const string EMAIL_NOT_CORRECT = "Email đã nhập không tồn tại";
        public const string PASS_NOT_CORRECT = "Mật khẩu đã nhập bị sai";
    }
    public interface IAuthService
    {
        Task<ApiResponse<string>> AdminLogin(AdminLogin adminLogin); //Chỉ dùng riêng cho admin
    }
    public class AuthService : IAuthService
    {
        private readonly IBaseRepository<Admin> _adminRepository;
        private readonly JwtSetting _jwtServerSetting;
        private readonly IValidator<AdminLogin> _adminLoginValidator;
        public AuthService(
            IBaseRepository<Admin> adminRepository,
            IOptions<JwtSetting> jwtServerSetting,
            IValidator<AdminLogin> adminLoginValidator
            )
        {
            _adminRepository = adminRepository;
            _jwtServerSetting = jwtServerSetting.Value;
            _adminLoginValidator = adminLoginValidator;
        }

        public async Task<ApiResponse<string>> AdminLogin(AdminLogin adminLogin)
        {
            try
            {
                var resultValidation = _adminLoginValidator.Validate(adminLogin);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<string>.FailtureValidation(resultValidation.Errors);
                }
                var adminInDb = await _adminRepository.GetAllQueryAble().
                    Where(e => e.Email == adminLogin.Email)
                    .FirstOrDefaultAsync();
                if (adminInDb == null)
                {
                    return new ApiResponse<string> { Message = [AuthError.EMAIL_NOT_CORRECT] };
                }
                bool isCorrectPass = BCrypt.Net.BCrypt.Verify(adminLogin.Password, adminInDb.Password);
                if (!isCorrectPass)
                {
                    return new ApiResponse<string> { Message = [AuthError.PASS_NOT_CORRECT] };
                }
                string encrypterToken = await JWTGenerator(new UserJwt
                    {
                        Email = adminInDb.Email,
                        Password = adminInDb.Password,
                        Role = Role.Admin,
                        Id = adminInDb.Id
                    }
                );
                return new ApiResponse<string> { Metadata = encrypterToken, IsSuccess = true };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<string> JWTGenerator(UserJwt userJwt)
        {
            try
            {
                var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Name, _jwtServerSetting.Subject),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                        new Claim("Id", userJwt.Id.ToString()),
                        new Claim("Email", userJwt.Email),
                        new Claim("Password", userJwt.Password),
                        new Claim("Role",userJwt.Role.ToString())
                    };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtServerSetting.Key));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _jwtServerSetting.Issuer,
                    _jwtServerSetting.Audience,
                    claims,
                    expires: DateTime.UtcNow.AddDays(7),
                    signingCredentials: signIn);

                var encrypterToken = new JwtSecurityTokenHandler().WriteToken(token);
                return encrypterToken;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
