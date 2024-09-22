using HRM.Repositories.Dtos.Models;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Requests
{
    public class AllowanceUpsertRequestExample : IExamplesProvider<AllowanceUpsert>
    {
        public AllowanceUpsert GetExamples()
        {
           return new AllowanceUpsert { 
               Name = "Fuel Allowance",
               Amount = 200000,
               Terms = "Term for fuel",
               ParameterName = "???"
           };
        }
    }
}
