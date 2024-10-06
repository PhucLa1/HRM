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
                        ManagerName = "Lã Hồng Phúc",
                        ManagerId = 1,
                    },
                    new DepartmentResult
                    {
                        Id = 2,
                        Name = "Engineering department",
                        ManagerName = "Nguyễn Thành Hưng",
                        ManagerId = 2,
                    },
                    new DepartmentResult
                    {
                        Id = 3,
                        Name = "System a department",
                        ManagerName = "Trịnh Gia Khánh",
                        ManagerId = 3,
                    }
                },
                IsSuccess = true,
            };
        }
    }
}
