using HRM.Repositories.Dtos.Models;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Requests
{
    public class BonusUpsertRequestExample : IExamplesProvider<BonusUpsert>
    {
        public BonusUpsert GetExamples()
        {
           return new BonusUpsert { 
                Name = "Bonus example",
                Amount = 100000,
                ParameterName = "PARAM_BONUS_EXAMPLE"
           };
        }
    }
}
