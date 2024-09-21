using HRM.Repositories.Dtos.Models;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Requests
{
    public class FomulaUpsertRequestExample : IExamplesProvider<FomulaUpsert>
    {
        public FomulaUpsert GetExamples()
        {
           return new FomulaUpsert { 
               Name = "Fomula example",
               FomulaDetail = "PARAM_BONUS_KPI10+PARAM_BONUS_PROJECTA-PARAM_DEDUCTION_LATE",
               Note = "Công thức tính lương mâux"
           };
        }
    }
}
