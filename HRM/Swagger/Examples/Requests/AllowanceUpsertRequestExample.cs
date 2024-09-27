using HRM.Repositories.Dtos.Models;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Requests
{
    public class AllowanceUpsertRequestExample : IExamplesProvider<AllowanceUpsert>
    {
        public AllowanceUpsert GetExamples()
        {
           return new AllowanceUpsert { 
               Name = "Phụ cấp nhiên liệu",
               Amount = 200000,
               Terms = "Nhân viên sẽ được hỗ trợ chi phí đi lại với mức phụ cấp nhiên liệu hoặc vé xe công cộng theo mức cố định hàng tháng",
               ParameterName = "???"
           };
        }
    }
}
