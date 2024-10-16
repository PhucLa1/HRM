using HRM.Repositories.Dtos.Results;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Responses
{
    public class DepartmentUserResponseExample : IExamplesProvider<ApiResponse<List<DepartmentUserResult>>>
    {
        public ApiResponse<List<DepartmentUserResult>> GetExamples()
        {
            return new ApiResponse<List<DepartmentUserResult>>
            {
                Metadata = new List<DepartmentUserResult>
                {
                    new DepartmentUserResult{Id = 1, Name = "Lã Hồng Phúc", Email = "phucminhbeos@gmail.com"},
                    new DepartmentUserResult{Id = 2, Name = "Nguyễn Thành Hưng", Email = "hugnt@gmail.com"}
                },
                IsSuccess = true,
            };
        }
    }
}
