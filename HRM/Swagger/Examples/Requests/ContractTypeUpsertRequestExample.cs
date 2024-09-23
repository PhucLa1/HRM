using HRM.Repositories.Dtos.Models;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Requests
{
    public class ContractTypeUpsertRequestExample : IExamplesProvider<ContractTypeUpsert>
    {
        public ContractTypeUpsert GetExamples()
        {
           return new ContractTypeUpsert { Name = "Fixed-term contract" };
        }
    }
}
