using HRM.Repositories.Dtos.Results;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Responses
{
    public class AccountInfoResponseExample : IExamplesProvider<ApiResponse<AccountInfo>>
    {
        public ApiResponse<AccountInfo> GetExamples()
        {
            return new ApiResponse<AccountInfo>
            {
                IsSuccess = true,
                Metadata = new AccountInfo
                {
                    Id = 1,
                    Role = Data.Entities.Role.Admin,
                    Name = "admin",
                    Email = "admin@gmail.com",
                }
            };
        }
    }
}
