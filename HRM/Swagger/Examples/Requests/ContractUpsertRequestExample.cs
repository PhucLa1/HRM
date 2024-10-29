using HRM.Data.Entities;
using HRM.Repositories.Dtos.Models;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Requests
{
    public class ContractUpsertRequestExample : IExamplesProvider<ContractUpsert>
    {
        public ContractUpsert GetExamples()
        {
            return new ContractUpsert
            {
                ContractSalaryId = 1,
                ContractTypeId = 1,
                DepartmentId = 1,
                PositionId = 1,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddYears(5),
                TypeContract = TypeContract.Partime,
                AllowanceIds = new List<int> { 1, 2 },
                InsuranceIds = new List<int> { 1, 2 },
                Name = "John Doe", 
                DateOfBirth = DateTime.Now.AddYears(-20),
                Gender = true,
                Address = "Ha Noi",
                CountrySide = "Hai Phong",
                NationalID = "008203000069",
                NationalStartDate = DateTime.Now.AddYears(-5),
                NationalAddress = "Hai Phong",
                Level = "Bachelor",
                Major = "Computer Science"
            };
        }
    }
}
