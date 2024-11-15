using HRM.Repositories.Dtos.Models;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Requests
{
    public class AccountUpdateRequestExample : IExamplesProvider<AccountUpdate>
    {
        public AccountUpdate GetExamples()
        {
            return new AccountUpdate { UserName = "Lã Hồng Phúc", Password = "Phucdeptrai", Email = "phucminhbeos@gmail.com" };
        }
    }
}
