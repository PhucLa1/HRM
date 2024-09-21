using HRM.Repositories.Dtos.Models;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Requests
{
    public class TaxRateUpsertRequestExample : IExamplesProvider<TaxRateUpsert>
    {
        public TaxRateUpsert GetExamples()
        {
           return new TaxRateUpsert { 
               Name = "Tax Rate example",
               Percent = 0.05,
               Condition = "Thu nhập tính thuế > ?tr theo luật mới",
               ParameterName = "PARAM_TAXDEDUCTION_EXAMPLE"
           };
        }
    }
}
