using HRM.Repositories.Dtos.Models;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Requests
{
    public class AccountLoginRequestExample : IExamplesProvider<AccountLogin>
    {
        public AccountLogin GetExamples()
        {
            return new AccountLogin { Email = "phucminhbeos@gmail.com", Password = "Phucdeptrai" };
        }
    }
}
