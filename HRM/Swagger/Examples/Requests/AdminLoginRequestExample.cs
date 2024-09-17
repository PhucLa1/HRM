using HRM.Repositories.Dtos.Models;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Requests
{
    public class AdminLoginRequestExample : IExamplesProvider<AdminLogin>
    {
        public AdminLogin GetExamples()
        {
            return new AdminLogin { Email = "phucminhbeos@gmail.com", Password = "Phucdeptrai" };
        }
    }
}
