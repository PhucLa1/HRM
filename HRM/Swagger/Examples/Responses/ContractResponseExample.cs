using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using HRM.Data.Entities;
using HRM.Repositories.Dtos.Results;
using NPOI.SS.Formula.Functions;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Responses
{
    public class ContractResponseExample : IExamplesProvider<ApiResponse<IEnumerable<ContractResult>>>
    {
        public ApiResponse<IEnumerable<ContractResult>> GetExamples()
        {
            return new ApiResponse<IEnumerable<ContractResult>>
            {
                Metadata = new List<ContractResult>
                {
                    new ContractResult
                    {
                        ContractSalaryId = 1,
                        ContractTypeId = 1,
                        DepartmentId = 1,
                        PositionId = 1,
                        StartDate = DateTime.Now,
                        EndDate = DateTime.Now.AddYears(5),
                        TypeContract = TypeContract.Partime,
                        Name = "Nguyen Van A",
                        DateOfBirth = DateTime.Now.AddYears(-20),
                        Gender = true,
                        Address = "Ha Noi",
                        CountrySide = "Hai Phong",
                        NationalID = "008203000069",
                        NationalStartDate = DateTime.Now.AddYears(-5),
                        NationalAddress = "Hai Phong",
                        Level = "Bachelor",
                        Major = "Computer Science",
                    },
                },
                IsSuccess = true,
            };
        }
    }
}
