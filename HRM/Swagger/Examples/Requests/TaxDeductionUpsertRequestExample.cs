using HRM.Repositories.Dtos.Models;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Requests
{
    public class TaxDeductionUpsertRequestExample : IExamplesProvider<TaxDeductionUpsert>
    {
        public TaxDeductionUpsert GetExamples()
        {
           return new TaxDeductionUpsert { 
               Name = "Tax Deduction example",
               FomulaType = 1,
               Terms = "Khoản giảm trừ thuế mới theo pháp luật",
               ParameterName = "PARAM_TAXDEDUCTION_EXAMPLE"
           };
        }
    }
}
