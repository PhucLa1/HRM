using HRM.Repositories.Dtos.Results;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Responses
{
    public class DepartmentResponseExample : IExamplesProvider<ApiResponse<IEnumerable<DepartmentResult>>>
    {
        public ApiResponse<IEnumerable<DepartmentResult>> GetExamples()
        {
            return new ApiResponse<IEnumerable<DepartmentResult>>
            {
                Metadata = new List<DepartmentResult>
                {
                    new DepartmentResult
                    {
                        Id = 1,
                        Name = "Human Resources department",
                        ManagerId = 101
                    },
                    new DepartmentResult
                    {
                        Id = 2,
                        Name = "Engineering department",
                        ManagerId = 102
                    },
                    new DepartmentResult
                    {
                        Id = 3,
                        Name = "System a department",
                        ManagerId = 103
                    }
                },
                IsSuccess = true,
            };
        }
    }
}
