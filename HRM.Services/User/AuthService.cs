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
        Task<ApiResponse<string>> AdminLogin(AccountLogin adminLogin); //Chỉ dùng riêng cho admin
        Task<ApiResponse<string>> EmployeeLogin(AccountLogin employeeLogin); //Dùng cho nhân viên
        Task<ApiResponse<AccountInfo>> GetCurrentAccount();
    }
    public class AuthService : IAuthService
    {
        private readonly IBaseRepository<Admin> _adminRepository;
        private readonly IBaseRepository<Employee> _employeeRepository;
        private readonly IBaseRepository<Contract> _contractRepository;
        private readonly JwtSetting _jwtServerSetting;
        private readonly IValidator<AccountLogin> _accountLoginValidator;
        public AuthService(
            IBaseRepository<Admin> adminRepository,
            IOptions<JwtSetting> jwtServerSetting,
            IValidator<AccountLogin> accountLoginValidator,
            IBaseRepository<Employee> employeeRepository,
            IBaseRepository<Contract> contractRepository
            )
        {
            _adminRepository = adminRepository;
            _jwtServerSetting = jwtServerSetting.Value;
            _accountLoginValidator = accountLoginValidator;
            _employeeRepository = employeeRepository;
            _contractRepository = contractRepository;
        }

        public async Task<ApiResponse<AccountInfo>> GetCurrentAccount()
        {
            try
            {
                var currentRole = _employeeRepository.Context
                    .GetCurrentUserRole();

                var currentId = _employeeRepository.Context
                    .GetCurrentUserId();
                string email = "";
                string name = "";
                TypeContract? type = null;
                if (currentRole == Role.Admin)
                {
                    var currentUser = await _adminRepository.GetAllQueryAble()
                        .Where(e => e.Id == currentId)
                        .FirstAsync();
                    email = currentUser.Email;
                    name = currentUser.Email.Split('@')[0];

                }
                else //Role là user
                {
                    var currentUser = await (from em in _employeeRepository.GetAllQueryAble()
                                             join c in _contractRepository.GetAllQueryAble() on em.ContractId equals c.Id
                                             where em.Id == currentId
                                             select new
                                             {
                                                 Name = c.Name,
                                                 Email = em.Email,
                                                 TypeContract = c.TypeContract
                                             }).FirstAsync();
                    email = currentUser.Email;
                    name = currentUser.Name;
                    type = currentUser.TypeContract;
                }
                var accountInfo = new AccountInfo
                {
                    Id = currentId,
                    Role = currentRole,
                    Name = name,
                    Email = email,
                    TypeContract = type == null ? 0 : type,
                };
                return new ApiResponse<AccountInfo>
                {
                    Metadata = accountInfo,
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiResponse<string>> AdminLogin(AccountLogin adminLogin)
        {
            try
            {
                var resultValidation = _accountLoginValidator.Validate(adminLogin);
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

        public async Task<ApiResponse<string>> EmployeeLogin(AccountLogin employeeLogin)
        {
            try
            {
                var resultValidation = _accountLoginValidator.Validate(employeeLogin);
                if (!resultValidation.IsValid)
                {
                    return ApiResponse<string>.FailtureValidation(resultValidation.Errors);
                }
                var employeeInDb = await _employeeRepository.GetAllQueryAble().
                    Where(e => e.Email == employeeLogin.Email)
                    .FirstOrDefaultAsync();
                if (employeeInDb == null)
                {
                    return new ApiResponse<string> { Message = [AuthError.EMAIL_NOT_CORRECT] };
                }
                bool isCorrectPass = BCrypt.Net.BCrypt.Verify(employeeLogin.Password, employeeInDb.Password);
                if (!isCorrectPass)
                {
                    return new ApiResponse<string> { Message = [AuthError.PASS_NOT_CORRECT] };
                }
                var typeContract = await _contractRepository
                    .GetAllQueryAble()
                    .Where(e => e.Id == employeeInDb.ContractId)
                    .Select(e => e.TypeContract)
                    .FirstAsync();
                string encrypterToken = await JWTGenerator(new UserJwt
                {
                    Email = employeeInDb.Email!,
                    Password = employeeInDb.Password!,
                    Role = typeContract == TypeContract.Partime ? Role.Partime : Role.FullTime,
                    Id = employeeInDb.Id
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
