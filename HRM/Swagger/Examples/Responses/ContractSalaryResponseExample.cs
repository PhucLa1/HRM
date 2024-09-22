using HRM.Repositories.Dtos.Results;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Responses
{
    public class ContractSalaryResponseExample : IExamplesProvider<ApiResponse<IEnumerable<ContractSalaryResult>>>
    {
        public ApiResponse<IEnumerable<ContractSalaryResult>> GetExamples()
        {
            return new ApiResponse<IEnumerable<ContractSalaryResult>>
            {
                Metadata = new List<ContractSalaryResult>
                {
                    new ContractSalaryResult
                    {
                        Id = 1,
                        BaseSalary = 5000000,
                        BaseInsurance = 1000000,
                        RequiredDays = 30,
                        RequiredHours = 8,
                        WageDaily = 200,
                        WageHourly = 30,
                        Factor = 1
                    },
                    new ContractSalaryResult
                    {
                        Id = 1,
                        BaseSalary = 8000000,
                        BaseInsurance = 1000000,
                        RequiredDays = 30,
                        RequiredHours = 8,
                        WageDaily = 200,
                        WageHourly = 30,
                        Factor = 1
                    },
                },
                IsSuccess = true,
            };
        }
    }
}
