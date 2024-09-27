using HRM.Repositories.Dtos.Models;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Requests
{
    public class DeductionUpsertRequestExample : IExamplesProvider<DeductionUpsert>
    {
        public DeductionUpsert GetExamples()
        {
           return new DeductionUpsert { 
                Name = "Deduction example",
                Amount = 100000,
                ParameterName = "PARAM_DEDUCTION_EXAMPLE"
           };
        }
    }
}
