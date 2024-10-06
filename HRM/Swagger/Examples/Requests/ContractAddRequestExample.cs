using HRM.Data.Entities;
using HRM.Repositories.Dtos.Models;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Requests
{
    public class ContractAddRequestExample : IExamplesProvider<ContractAdd>
    {
        public ContractAdd GetExamples()
        {
            return new ContractAdd
            {
                ContractSalaryId = 1,
                ContractTypeId = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddYears(5),
                TypeContract = TypeContract.Partime,
                AllowanceIds = new List<int> { 1, 2 },
            };
        }
    }
}
