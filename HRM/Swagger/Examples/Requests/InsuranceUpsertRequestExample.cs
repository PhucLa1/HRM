using HRM.Repositories.Dtos.Models;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Requests
{
    public class InsuranceUpsertRequestExample : IExamplesProvider<InsuranceUpsert>
    {
        public InsuranceUpsert GetExamples()
        {
           return new InsuranceUpsert { 
               Name = "Health insurance",
               PercentEmployee = 10,
               PercentCompany = 20,
               ParameterName = "???",
           };
        }
    }
}
