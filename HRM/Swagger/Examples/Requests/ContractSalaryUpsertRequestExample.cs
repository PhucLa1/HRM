using HRM.Repositories.Dtos.Models;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Requests
{
    public class ContractSalaryUpsertRequestExample : IExamplesProvider<ContractSalaryUpsert>
    {
        public ContractSalaryUpsert GetExamples()
        {
           return new ContractSalaryUpsert { 
               BaseInsurance = 5000000,
               BaseSalary = 5000000,
               RequiredDays = 30,
               RequiredHours = 8,
               WageDaily = 200,
               WageHourly = 30,
               Factor = 1
           };

        }
    }
}
