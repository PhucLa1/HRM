using HRM.Repositories.Dtos.Models;
using Swashbuckle.AspNetCore.Filters;

namespace HRM.Apis.Swagger.Examples.Requests
{
    public class HistoryUpsertRequestExample : IExamplesProvider<HistoryUpsert>
    {
        public HistoryUpsert GetExamples()
        {
            return new HistoryUpsert { StatusHistory = Data.Entities.StatusHistory.In, TimeSweep = new DateTime(2024, 10, 29, 12, 0, 0) };
        }
    }
}
